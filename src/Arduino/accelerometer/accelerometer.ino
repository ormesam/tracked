// Tested on Arduino Nano 33 IOT

#include <ArduinoBLE.h>
#include <Arduino.h>
#include "LSM6DS3.h"
#include "Wire.h"

BLEService accService("19B10001-E8F2-537E-4F6C-D104768A1214");
BLEStringCharacteristic accCharacteristic("19B10001-E8F2-537E-4F6C-D104768A1214", BLERead | BLENotify, 15);

const int ledPin = LED_BUILTIN;

void setup() {
  Serial.begin(9600);
  Wire.begin();
  accelerometer.begin();
  
  if(accelerometer.isActive()){
    Serial.println("Accelerometer already active");
  }else{
    if(accelerometer.powerOn()){
      Serial.println("Accelerometer Power ON");
    }else{
      Serial.println("Accelerometer Not Powered On");
    }
  }

  accelerometer.changeFullScale(XL_FS_16G);
  accelerometer.changeOutputDataRate(POWER_416_HZ);

  // set LED pin to output mode
  pinMode(ledPin, OUTPUT);

  // begin initialization
  if (!BLE.begin()) {
    Serial.println("Starting BLE failed!");

    while (1);
  }

  // set advertised local name and service UUID:
  BLE.setLocalName("Tracked");
  BLE.setAdvertisedService(accService);

  // add the characteristic to the service
  accService.addCharacteristic(accCharacteristic);

  // add service
  BLE.addService(accService);

  // set the initial value for the characeristic:
  accCharacteristic.writeValue("");

  // start advertising
  BLE.advertise();

  Serial.println("Starting Tracked");
}

void loop() {
  BLEDevice central = BLE.central();

  if (central) {
    while (central.connected()) {
          float x = accelerometer.getConvertedXAxis();
          float y = accelerometer.getConvertedXAxis();
          float z = accelerometer.getConvertedXAxis();

          String buf;
            buf += String(roundf(x*100.0)/100.0);
            buf += F(",");
            buf += String(roundf(y*100.0)/100.0);
            buf += F(",");
            buf += String(roundf(z*100.0)/100.0);
          
          accCharacteristic.setValue(buf);

          delay(100);
    }
  }
}
