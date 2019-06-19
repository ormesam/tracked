#include <SoftwareSerial.h>

SoftwareSerial BT(10, 11);
char command;

void setup()
{
  // set digital pin to control as an output
  pinMode(13, OUTPUT);
  // set the data rate for the SoftwareSerial port
  BT.begin(9600);
}

void loop()
{
  // check if text arrived in from BT serial...
  if (BT.available())
  {
    command = BT.read();

    if (command == 'r') {
      // run...
    }
    
    if (command == 'x') {
      // exit...
    }
  }
  
  BT.println("123,123,123"); // replace this with the data from an accelerometer
  
  delay(250);
}
