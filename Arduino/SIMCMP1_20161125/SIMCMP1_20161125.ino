/******************************************************************************
SIMCMP1 20161125, Arduino Comm Annunciator Panel for FSX/P3D
 Source   :  www.simav8.com (c) Brian McMullan 2016
 CPU:     :  Arduino MEGA 2560 CPU (overkill, but use in all SIMAV8 projects for consistency)
 switches :  Eight momentary switches (eSwitch 5501 series)
 SW01-07  :  COM1, COM2, BOTH, NAV1, NAV2,  MKR, DME, GPS/NAV (toggle like function)
 LEDs     :   A,    F,    B,    G,    C,     E,   DP    on   DIG4
 LEDs     :  GPS/NAV are DIGxx on SEG xx and xx
 Link2FS  :  MUST use "Include CR/LF at end" or won't work, this code relies on NL char (dec 10) for EOL
*******************************************************************************/

String ProjVersion = "SIMCMP1_20161125";

// LIBRARIES
// #include "math.h"            // quadrature needs math lib
#include "SoftReset.h"          // SoftReset lib,   https://github.com/WickedDevice/SoftReset
#include "LedControl.h"         // MAX7219 library, http://www.wayoda.org/arduino/ledcontrol/index.html

// ~~~~~~~ MAX7219 pins
const unsigned int MAX7219_PIN_MOSI = 8;
const unsigned int MAX7219_PIN_CS   = 9;
const unsigned int MAX7219_PIN_CK   = 10;
const unsigned int MAX7219_NUMCHIPS = 1;  // one chip

LedControl lc=LedControl(MAX7219_PIN_MOSI, MAX7219_PIN_CK, MAX7219_PIN_CS, MAX7219_NUMCHIPS);
int LEDIntensity = 10;

// PIN CONFIGURATION

// bytes for LED "accumulator" function
byte DIG0_LEDs=0;
byte DIG1_LEDs=0;
byte DIG4_LEDs=0;
byte DIG5_LEDs=0;
byte DIG6_LEDs=0;
byte DIG7_LEDs=0;
int  OMI_LED=0;

boolean bLFuelLow    = false;
boolean bRFuelLow    = false;
boolean bLVACLow     = false;
boolean bRVACLow     = false;
boolean bOILPRESSLow = false;
boolean bVOLTSLow    = false;

// bytes for MAX7219 digits
byte SEGA = B01000000;  // a
byte SEGF = B00000010;  // f
byte SEGG = B00000001;  // g
byte SEGB = B00100000;  // b
byte SEGC = B00010000;  // c
byte SEGE = B00000100;  // e
byte SEGDP =B10000000;  // DP
byte SEGD = B00001000;  // d

// Typedef for switch data structure used in function ProcessMomentary
//   swmom_  for functions, SW_MOMENTARY_TYPE for variables
typedef struct swmom_ {
  int pin;
  unsigned long millis;
  int state;
} SW_MOMENTARY_TYPE;

// Switch variables, structures
//                 var,    pin, 0, 0
SW_MOMENTARY_TYPE SW_S1 = { 15, 0, 0};
SW_MOMENTARY_TYPE SW_S2 = { 14, 0, 0};
SW_MOMENTARY_TYPE SW_S3 = { 2, 0, 0};
SW_MOMENTARY_TYPE SW_S4 = { 3, 0, 0};
SW_MOMENTARY_TYPE SW_S5 = { 4, 0, 0};
SW_MOMENTARY_TYPE SW_S6 = { 5, 0, 0};
SW_MOMENTARY_TYPE SW_S7 = { 6, 0, 0};
SW_MOMENTARY_TYPE SW_S8 = { 7, 0, 0};

unsigned int SW_S8_GPSNAV = 0;

// debounce and pushed long time values
long               debouncesw=50;               // switch debounce time in milliseconds (1000 millis per second)
const unsigned int PUSH_LONG_LEVEL1  = 2000;    // Encoder Switch pressed LONG time value (5000 = five seconds)
unsigned long      millisSinceLaunch = 1;       // previous reading of millis, seed with 1 to init, set to 0 if don't want to use
unsigned long      delaytime=250;
unsigned long      shortdel=50;
unsigned long      mydelay=100;

//  Main loop millies comparators  
unsigned long ul_PreviousMillis;
unsigned long ul_CurrentMillis;

// Pin 13 has an LED connected on most Arduino boards, light it up
int led = 13;

// variables for master and avionics switches or states, test state, etc
boolean gbMasterVolts=false;
boolean gbAvionicsVolts=false;
boolean gbMasterVoltsPrev=false;
boolean gbAvionicsVoltsPrev=false;
boolean gbTestLight=false;
boolean gbTestLightChanged=false;

// variables for SerialIO (reserve 200 chars in SETUP)
String  SerialInString = "";        // a string to hold incoming data (reserved in SETUP)
boolean SerialInReady = false;      // whether the string is complete
String  SerialCmdString = "";       // a string to hold broken up commands (reserved in SETUP)
boolean SerialCmdReady = false;     // whether the command is ready

// variable for watchdog timer (eg SIM up or down)
long lWatchDog;
int  iCntWatchDog;
int  giDoingDemo;

/*
 ####  ###### ##### #    # #####  
#      #        #   #    # #    # 
 ####  #####    #   #    # #    # 
     # #        #   #    # #####  
#    # #        #   #    # #      
 ####  ######   #    ####  #      
*/
 
 void setup() {

  // Light up the LED on pin 13
  // pinMode(led, OUTPUT); 
  // digitalWrite(led, HIGH);           // turn the LED on (HIGH is the voltage level)

  // MAX7219 wake up
  lc.shutdown(0,false);              // wake up MAX7219
  lc.setIntensity(0,10);             // set intensity
  lc.clearDisplay(0);                // clear display

  // Switches, set the pins for pullup mode
  pinMode(SW_S1.pin, INPUT_PULLUP);
  pinMode(SW_S2.pin, INPUT_PULLUP);
  pinMode(SW_S3.pin, INPUT_PULLUP);
  pinMode(SW_S4.pin, INPUT_PULLUP);
  pinMode(SW_S5.pin, INPUT_PULLUP);
  pinMode(SW_S6.pin, INPUT_PULLUP);
  pinMode(SW_S7.pin, INPUT_PULLUP);
  pinMode(SW_S8.pin, INPUT_PULLUP);  

  // light GPS on GPS/NAV
  // lc.setRow(0,0, SEGE);   SW_S8_GPSNAV=1;
 
  // Seed the millies
  // BlinkLast = lWatchDog = ul_PreviousMillis = millis();
  // BlinkLast = millis();
  // lWatchDog = millis();

  // ********* Serial communications to PC
  Serial.begin(115200);  // output
  SerialInString.reserve(200);        // reserve 200 bytes for the inputString
  SerialCmdString.reserve(200);       // reserve another 200 for post processor
  /*
  while (!Serial) {
    ; // wait for serial port to connect (you have to send something to Arduino)
      // I just ignore it and press on assuming somethings listening
  }
  if (Serial.available() > 0) {
  */  
    // whenever DTR brought high on PC side, this will spit out and program resets
    // PC side, VB.NET..   SerialPort1.DtrEnable = True      'raise DTR
    Serial.println(ProjVersion);
    // Serial.println(millisSinceLaunch);
    // Serial.println("REF");     // request all new data from SIM
  // }

  // flicker everything really quickly just to show it's running to user
  int disp = 0;
  // light up everything
  lc.setRow(disp,4,B11111111);   // SWITCH LEDS
  lc.setRow(disp,5,B11111111);   // TOP ROW LEDS
  lc.setRow(disp,1,B11111111);   // BOT ROW LEDS
  lc.setRow(disp,7,B11111111);   // OMI ROW LEDS
  lc.setRow(disp,0,B11111111);   // GPS/NAV switch

  // Wait X milliseconds then powerdown displays and wait for commands from host
  // turn everything off
  lc.setRow(disp,4,B00000000);   // SWITCH LEDS
  lc.setRow(disp,5,B00000000);   // TOP ROW LEDS
  lc.setRow(disp,1,B00000000);   // BOT ROW LEDS
  lc.setRow(disp,7,B00000000);   // OMI ROW LEDS
  lc.setRow(disp,0,B00000000);   // GPS/NAV switch

} // end of SETUP 


/* 
#       ####   ####  #####  
#      #    # #    # #    # 
#      #    # #    # #    # 
#      #    # #    # #####  
#      #    # #    # #      
######  ####   ####  #      
*/

void loop() { 

  // time snapshot, same millis value throughout this entire loop
  ul_CurrentMillis = millis();

  // *************************************************************
  // READ and PARSE SERIAL INPUT
  // void SerialEvent used to gather data up to NL
  // *************************************************************

  // *************************************************************
  // READ and PARSE SERIAL INPUT (version 2)
  // Jim's program (Link2FS) sends commands batched up /A1/F1/J1/K1 (no NL separator)
  // added IsSerialCommand routine to fetch them one at a time up to separator like / or =
  // leaving rest on serialbuffer for next loop to process
  // *************************************************************
  if ( IsSerialCommand() )  { 
    int iLen = SerialCmdString.length();
    // Serial.print("Got string len "); Serial.println(iLen); Serial.print("Got: [");  Serial.print(SerialCmdString); Serial.println("]");
    char cToken; char cAction; char *cParam;
    cToken   = SerialCmdString[0];    
    cAction  = SerialCmdString[1];    
    cParam = &(SerialCmdString[2]);   //cParam is a pointer, assigned to address of 3rd char in SerialCmdString
    //Serial.print("cToken  = "); Serial.println(cToken);
    //Serial.print("cAction = "); Serial.println(cAction);
    //Serial.print("cParam  = "); Serial.println(cParam);
    //Quick monitor
    //Serial.print("TAP ["); Serial.print(cToken); Serial.print(cAction); Serial.print(cParam); Serial.println("]");
    // cToken  ?, /, <, = etc
    // cAction M, J, E, etc
    // cParam  0, 1, or longer values  
      
    // Process the command sent in
    switch (cToken) {
     
      case '?' :  {    // case cToken = ?
        Serial.println("");
        Serial.println("------ INPUTS ------");
        Serial.println(" Power           @Pn  (0/1)");      
        Serial.println(" REBOOT Arduino  @R");
        Serial.println(" TEST Mode       @Tn (0/1)");
        Serial.println(" Brightness      @Bnn (0-15) [10]");              
        Serial.println(" MASTER power    <an  (0/1)");
        Serial.println(" AVIONICS power  <gn  (0/1)");
        Serial.println(" Switch lights   =Mn=Nn=On=Pn=Qn=Rn (0/1)");
        Serial.println(" NAV GPS switch  =ln (0/1)");
        Serial.println(" OMI lights      =Vn (0-3)");
        Serial.println(" FUEL L, R       /Jn /Kn");
        Serial.println(" VACUUM          /Nn");
        Serial.println(" OIL PRESS       /Fn");
        Serial.println(" VOLTS           /Rn");
        Serial.println("------ OUTPUTS ------");
        Serial.println(" Button Presses  (try them)");
        Serial.println("");
        }
        break;
      
      // NEXT TOKEN TO PROCESS "<"
      case '<' : {   // case cToken = <
        switch (cAction) {    //MASTER switch 
          case 'a' : if (cParam[0]=='0') { gbMasterVolts=false; }
                     if (cParam[0]=='1') { gbMasterVolts=true; }
                     break;   //AVIONICS switch
          case 'g' : if (cParam[0]=='0') { gbAvionicsVolts=false; }
                     if (cParam[0]=='1') { gbAvionicsVolts=true; }
                     break;
          default:   // if other commands come in, ignore them
                     Serial.println("Err <  '" + SerialCmdString + "'"); 
                     //Serial.println("!<ERR!");
                     break;
        } // end switch cAtion
      } // end switch cToken is '<'
      break;  // break case cToken '<'

      // NEXT cToken to Process '='
      case '=' :  {
        switch (cAction) {    // =M1=N0=01=P1 etc, switch functions lights, set the DIG4, SEGx flags
          case 'M' : if (cParam[0]=='1') { DIG4_LEDs |= SEGA; } else { DIG4_LEDs &= ~SEGA; } break;   // COM1
          case 'N' : if (cParam[0]=='1') { DIG4_LEDs |= SEGF; } else { DIG4_LEDs &= ~SEGF; } break;   // COM2
          case 'O' : if (cParam[0]=='1') { DIG4_LEDs |= SEGB; } else { DIG4_LEDs &= ~SEGB; } break;   // BOTH 
          case 'P' : if (cParam[0]=='1') { DIG4_LEDs |= SEGG; } else { DIG4_LEDs &= ~SEGG; } break;   // NAV1
          case 'Q' : if (cParam[0]=='1') { DIG4_LEDs |= SEGC; } else { DIG4_LEDs &= ~SEGC; } break;   // NAV2
          case 'U' : if (cParam[0]=='1') { DIG4_LEDs |= SEGE; } else { DIG4_LEDs &= ~SEGE; } break;   // MKR 
          case 'R' : if (cParam[0]=='1') { DIG4_LEDs |= SEGDP; } else { DIG4_LEDs &= ~SEGDP; } break; // DME    
          // GPSNAV switch set DIG0, SEGx flags
          case 'l' : if (cParam[0]=='1') { DIG0_LEDs = SEGE; } else { DIG0_LEDs = SEGDP; } break;  // GPS else NAV
          
          // OMI, 0 = none, 1 = outer (SEGA), 2 = middle (SEGF), 3 = inner (SEGB)
          case 'V' : if (cParam[0]=='0') { DIG7_LEDs = 0; }       // none
                     if (cParam[0]=='1') { DIG7_LEDs = SEGA; }    // 1= outer
                     if (cParam[0]=='2') { DIG7_LEDs = SEGF; }    // 2= middle
                     if (cParam[0]=='3') { DIG7_LEDs = SEGB; }    // 2= middle                     
                     break;
          default:   // if other commands come in, ignore them
            // Serial.println("invalid = cmd, use '?' for help..."); 
            Serial.println("Err =  '" + SerialCmdString + "'");             
            break; 
        } // end switch (cAction)
      } // end switch Token is '='
      break;  // break case cToken '='
      
      // NEXT cToken to Process '/'
      case '/' :  {
        switch (cAction) {     
          case 'J' : if (cParam[0]=='1') { bLFuelLow = true; } else { bLFuelLow=false; } break;         // left fuel low
          case 'K' : if (cParam[0]=='1') { bRFuelLow = true; } else { bRFuelLow=false; } break;         // right fuel low
          case 'N' : if (cParam[0]=='1') { bLVACLow = true; } else { bLVACLow = false; }                // Vacuum
                     bRVACLow = bLVACLow;  // only one signal from FSX it seems...                     
                     break;
          case 'F' : if (cParam[0]=='1') { bOILPRESSLow = true; } else {bOILPRESSLow = false; } break;  // OIL PRESSURE
          case 'R' : if (cParam[0]=='1') { bVOLTSLow = true; } else {bVOLTSLow = false; } break;  // VOLTS

          default:   // if other commands come in, ignore them
            Serial.println("Err /  '" + SerialCmdString + "'"); 
            // Serial.println("invalid / cmd, use '?' for help..."); 
            break; 
        } // end switch (cAction)
      } // end switch Token is '/'
      break;  // break case cToken '/'

      
      // My custom tokens "@" (not on Jim's software)
      case '@' :   {
        switch (cAction) {
          case 'm' : // bmm test
                     Serial.print("gbMasterVolts   "); Serial.println(gbMasterVolts);
                     Serial.print("gbAvionicsVolts "); Serial.println(gbAvionicsVolts);
                     //Serial.print("BOTHwereON   "); Serial.println(gbMasterAndAvionicsWereBothON);
                     break;
          case 'T' : // @T, test / demo, light up all LEDs
                     if (cParam[0]=='1') { gbTestLight=true; } else { gbTestLight=false; }
                     gbTestLightChanged = true;
                     break;
          case 'P' : // @P, Unit power on or off (really, don't use, rely on <a_ and <g_)
                     if (cParam[0]=='0') { gbMasterVolts=false; gbAvionicsVolts=false; }  
                     if (cParam[0]=='1') { gbMasterVolts=true; gbAvionicsVolts=true; }    
                     break;

          case 'R': // @R    Software reset the Arduino
                    Serial.println("Software resetting Arduino...");
                    soft_restart();
                    break;
          case 'B' : { // @B, Brightness, 0 - 15 (zero is NOT off!)
                      int iScratch = atoi(cParam);    
                      if (iScratch >= 0 && iScratch <= 15) {
                        LEDIntensity = iScratch;
                        for (int i=0; i<MAX7219_NUMCHIPS; i++) { lc.setIntensity(i,LEDIntensity); }
                      }                         
                    }
                    break;
                    
          default:  // if other commands come in, ignore them
                   //Serial.println("invalid @ cmd, use '?' for help..."); 
                   Serial.println("Err @  '" + SerialCmdString + "'");                    
                   //Serial.println("!@ERR!");
                   break;
        }  // end code block
      } // end switch Token is '@', cAction
      break;  // break case cToken @

      // ANY OTHER CTOKEN (unrecognized)
      default:   
          Serial.println("Err '" + SerialCmdString + "'"); 
          // ignore null entry, otherwise 
          /*
          if (int(cToken) == 0 ) {
              Serial.println("use '?' for help"); 
          } else {
              Serial.println("invalid token, use '?' for help"); 
          }
          */
          break;
    } // end switch cToken

    //
    // post processing, if any
    //
        
  } // END if SerialInReady


  // *************************************************************
  // Process LED flags and light up displays as needed
  // *************************************************************
  int disp=0;
  
  // if in testmode, light it up
  if (gbTestLight && gbTestLightChanged ) {  // light up everything
    lc.setRow(disp,4,B11111111);   // SWITCH LEDS
    lc.setRow(disp,5,B11111111);   // TOP ROW LEDS
    lc.setRow(disp,1,B11111111);   // BOT ROW LEDS
    lc.setRow(disp,7,B11111111);   // OMI ROW LEDS
    lc.setRow(disp,0,B11111111);   // GPS/NAV switch
    gbTestLightChanged = false;
  }

  // NOT IN TEST MODE, DO LIGHTS AS NEEDED
  //  IF MASTER POWER,   THEN ANNUNCIATORS CAN WORK (ELSE BLACK)
  //  IF AVIONICS POWER, THEN SWITCHES CAN WORK (ELSE OFF)
  if (!gbTestLight) {

    if ( gbTestLightChanged ) {    // if came out of test mode, turn everything off
      lc.setRow(disp,4,B00000000);   // SWITCH LEDS
      lc.setRow(disp,5,B00000000);   // TOP ROW LEDS
      lc.setRow(disp,1,B00000000);   // BOT ROW LEDS
      lc.setRow(disp,7,B00000000);   // OMI ROW LEDS
      lc.setRow(disp,0,B00000000);   // GPS/NAV switch
      gbTestLightChanged = false;
    }
  
    // If MASTER voltage, then do Annunciator lights 
    if ( gbMasterVolts) {
      DIG1_LEDs = DIG5_LEDs = 0;                              // reset LED accumulators
      if (bLFuelLow) { DIG1_LEDs |= SEGA | SEGF | SEGB; }     // left fuel low
      if (bRFuelLow) { DIG1_LEDs |= SEGF | SEGB | SEGG; }     // right fuel low
      if (bLVACLow)  { DIG5_LEDs |= SEGA | SEGF | SEGB; }     // left vacuum low
      if (bRVACLow)  { DIG5_LEDs |= SEGF | SEGB | SEGG; }     // right vacuum low  
      if (bOILPRESSLow) { DIG5_LEDs |= SEGC | SEGE | SEGDP; } // OIL PRESSURE
      if (bVOLTSLow)    { DIG1_LEDs |= SEGC | SEGE | SEGDP; } // VOLTS
      lc.setRow(disp,5,DIG5_LEDs);   // TOP ROW LEDS          // send signal to MAX7219
      lc.setRow(disp,1,DIG1_LEDs);   // BOT ROW LEDS          // send signal to MAX7219
    } else {  // no master volts, go dark
      lc.setRow(disp,5,B00000000);   // TOP ROW LEDS
      lc.setRow(disp,1,B00000000);   // BOT ROW LEDS
    }      

    // if AVIONICS voltage, then do button and OMI lights
    if ( gbAvionicsVolts) {          // light up just what's needed
      lc.setRow(disp,4,DIG4_LEDs);   // SWITCH LEDS
      lc.setRow(disp,7,DIG7_LEDs);   // OMI ROW LEDS
      lc.setRow(disp,0,DIG0_LEDs);   // GPS/NAV switch
    } else {
      lc.setRow(disp,4,B00000000);   // SWITCH LEDS
      lc.setRow(disp,7,B00000000);   // OMI ROW LEDS
      lc.setRow(disp,0,B00000000);   // GPS/NAV switch
    }     

  } // end    if (!gbTestLight) 

  // *************************************************************
  // Respond to switch button presses
  // *************************************************************

  // Check using pointers instead of big chunks o code
  ProcessMomentary(&SW_S1);          // COM1
  ProcessMomentary(&SW_S2);          // COM2
  ProcessMomentary(&SW_S3);          // BOTH
  ProcessMomentary(&SW_S4);          // NAV1
  ProcessMomentary(&SW_S5);          // NAV2 
  ProcessMomentary(&SW_S6);          // MKR 
  ProcessMomentary(&SW_S7);          // DME
  ProcessMomentary(&SW_S8);          // GPS/NAV

  // ***************************************************
  // Switch action, state==2 means pressed and debounced, ready to read
  // ***************************************************

  // See if user wanted manual demo mode
  // Press and HELD both first (COM1) and last (DME) switches for a while  
  if (SW_S1.state==4 && SW_S7.state==4) {   // see if switches held "long press"
    SW_S1.state=SW_S1.state=5;              // increment state of the switch presses
    gbTestLight = !gbTestLight;             // toggle gbTestLight
    gbTestLightChanged = true;              // flag as having changed (flag keeps from flickering in / out test mode)
  }

  // Act on regular button presses
  if(SW_S1.state==2) { Serial.println("A45"); SW_S1.state=3; }   // COM1
  if(SW_S2.state==2) { Serial.println("A46"); SW_S2.state=3; }   // COM2
  if(SW_S3.state==2) { Serial.println("A47"); SW_S3.state=3; }   // BOTH
  if(SW_S4.state==2) { Serial.println("A48"); SW_S4.state=3; }   // NAV1
  if(SW_S5.state==2) { Serial.println("A49"); SW_S5.state=3; }   // NAV2
  if(SW_S6.state==2) { Serial.println("A53"); SW_S6.state=3; }   // MKR
  if(SW_S7.state==2) { Serial.println("A50"); SW_S7.state=3; }   // DME
  if(SW_S8.state==2) { Serial.println("A54"); SW_S8.state=3; }   // NAV/GPS Toggle
   
} // end main loop


/*
###### #    # #    #  ####  ##### #  ####  #    #  ####  
#      #    # ##   # #    #   #   # #    # ##   # #      
#####  #    # # #  # #        #   # #    # # #  #  ####  
#      #    # #  # # #        #   # #    # #  # #      # 
#      #    # #   ## #    #   #   # #    # #   ## #    # 
#       ####  #    #  ####    #   #  ####  #    #  ####  
*/

/*-----------------------------------
 ProcessMomentary(pointer to swmom_ struct)
  Brian McMullan SIMAV8 2016
  Processes the specified switch state of momentary buttons
  Read the state of the switch to get results
  - state 0, switch not pressed
  - state 2, switch pressed and debounced (short press)
  - state 4, switch pressed and held for PUSH_LONG_LEVEL1 (eg 3 seconds, 5 seconds)
  - state 6, switch pressed and held for 2x PUSH_LONG_LEVEL1
------------------------------------*/
void ProcessMomentary(struct swmom_ *AnySwitch)
{
  if (digitalRead(AnySwitch->pin) == LOW) {       // switch pressed
    switch(AnySwitch->state) {
      case 0: /* first instance of press */ AnySwitch->millis = ul_CurrentMillis; AnySwitch->state = 1; break;
      case 1: /* debouncing counts */       if (ul_CurrentMillis - AnySwitch->millis > debouncesw) { AnySwitch->state = 2; break; }
      case 2: /* DEBOUNCED, read valid */   break;
      case 3: /* For LONG press */          if (ul_CurrentMillis - AnySwitch->millis > PUSH_LONG_LEVEL1) { AnySwitch->state = 4; break; }
      case 4: /* LONG PRESS, read valid */  break;
      case 5: /* For LONG * 2 press */      if (ul_CurrentMillis - AnySwitch->millis > (PUSH_LONG_LEVEL1*2)) { AnySwitch->state = 6; break; }
      case 6: /* LONG PRESS * 2, read  */   break;
      case 7: /* LONG PRESS * 2, hold  */   break;      
    } // end switch
  } else {    // button went high, reset the state machine
    AnySwitch->state = 0;
  }
}

/*********************************************************************************************************
SerialEvent gathers incoming serial string, but with Jim's can be multiple commands per line like
  =M0=N1=O0
that needs to be broken up and fetched as =M0, then =N1, then =O0 as if came that way
this routine does that breaking up
*********************************************************************************************************/
boolean IsSerialCommand() {
  // sees if serial data is available (full line read up to NL)
  // busts it up as individual commands if so and feeds back to caller
  int iSISptr = 0;
  char cSISchar;
  int iLenTemp = SerialInString.length();
  String sTempStr;
  
  SerialCmdString = "";      // clear the command buffer  

  if (SerialInReady) {       // data on the In buffer?
    //testing
    //Serial.print("ISC [");Serial.print(SerialInString); Serial.println("]");
    
    // copy from serial buffer up any tokens OR NL
    do {    // always do the first char
      SerialCmdString += SerialInString[iSISptr];    // add char to command
      iSISptr++;                                     // point to next char on IN buffer
      cSISchar = SerialInString[iSISptr];            // store next char
      //   keep doing this till hit a token or NL
    } while (cSISchar != '/' && cSISchar != '=' && cSISchar != '<' && cSISchar != 10 && cSISchar != 0);

    // copy primary string into a temp buffer CHOPPING off command already read 
    sTempStr = &(SerialInString[iSISptr]);
    // copy temp buffer back to primary buffer (shortening it)
    SerialInString = sTempStr;

    // if finally emptied the existing IN buffer, set flag so more can be read
    if (SerialInString.length() == 0) {  SerialInReady = false;}
    return(true);
    
  } else {
    return(false);
  }
} // end function

/*********************************************************************************************************
SerialEvent occurs whenever a new data comes in the hardware serial RX.  This routine is run each
time loop() runs, so using delay inside loop can delay response.  Multiple bytes of data may be available.
 reads one char at a time
 when NL is found
   calls that string ready for processing
   sets SerialInReady to true
   no more chars will be read while SerialInReady is true
 to break up single line multiple command strings, use external routine
*********************************************************************************************************/
void serialEvent() {
  while (Serial.available() && !SerialInReady) {
      // get the new byte(s), stopping when CR or LF read
      char inChar = (char)Serial.read(); 
      // static boolean EOL = false;
   
      // debugging, dump every character back
      //Serial.print("[");  Serial.print(inChar,DEC); Serial.println("]");

      // Windows Serial.writeline sends NL character (ASCII 10)
      // Putty and other terms send CR character (ASCII 13)
      
      if (inChar == 13) { break; }      // ignore CR

      // if NL (aka LF, aka ASCII 10) call that end of string, flag it
      if (inChar == 10) {
         SerialInReady = true;
         // if want to echo string back to sender, uncomment this
         // Serial.println(SerialInString);
      } else {
         // add it to the SerialInString:
         SerialInString += inChar;
      }
   } // end while
} // end SerialEvent handler

