#include <SoftwareSerial.h>
#include<Wire.h>

SoftwareSerial BT(10, 11);
char command;
const int MPU_addr=0x68;  // I2C address of the MPU-6050
int AcX,AcY,AcZ,Tmp,GyX,GyY,GyZ;

void setup()
{
  // set the data rate for the SoftwareSerial port
  BT.begin(9600);
  
  Wire.begin();
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x6B);  // PWR_MGMT_1 register
  Wire.write(0);     // set to zero (wakes up the MPU-6050)
  Wire.endTransmission(true);
  Serial.begin(9600);
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
  
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x3B);  // starting with register 0x3B (ACCEL_XOUT_H)
  Wire.endTransmission(false);
  Wire.requestFrom(MPU_addr,14,true);  // request a total of 14 registers
  AcX=Wire.read()<<8|Wire.read();  // 0x3B (ACCEL_XOUT_H) & 0x3C (ACCEL_XOUT_L)    
  AcY=Wire.read()<<8|Wire.read();  // 0x3D (ACCEL_YOUT_H) & 0x3E (ACCEL_YOUT_L)
  AcZ=Wire.read()<<8|Wire.read();  // 0x3F (ACCEL_ZOUT_H) & 0x40 (ACCEL_ZOUT_L)
  Tmp=Wire.read()<<8|Wire.read();  // 0x41 (TEMP_OUT_H) & 0x42 (TEMP_OUT_L) UNUSED FOR NOW
  GyX=Wire.read()<<8|Wire.read();  // 0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L) UNUSED FOR NOW
  GyY=Wire.read()<<8|Wire.read();  // 0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L) UNUSED FOR NOW
  GyZ=Wire.read()<<8|Wire.read();  // 0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L) UNUSED FOR NOW
  
  String buf;
  buf += String(AcX);
  buf += F(",");
  buf += String(AcY);
  buf += F(",");
  buf += String(AcZ);
  
  BT.println(buf); // replace this with the data from an accelerometer
  
  delay(250);
}
