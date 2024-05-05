// #define DEBUGGING
#include "BluetoothSerial.h"
#include "config.h"

#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED)
#error Bluetooth is not enabled! Please run `make menuconfig` to and enable it
#endif

#if defined(DEBUGGING)
#define DBG(msg) SERIAL_BT.println(msg)
#else
#define DBG(msg)
#endif

#define EBIKE_COMM_BAUDRATE 1200
#define SERIAL_BT SerialBT

BluetoothSerial SERIAL_BT;
bool is_passthrough_mode;

void set_motor(bool enable) {
  // Apparently the relay uses inverse logic
  digitalWrite(PIN_MOTOR_ENABLE_OUT, !enable);
}

bool wait_for_passthrough()
{
  auto start_time = millis();
  auto current_time = start_time;

  DBG("Waiting if display or bluetooth connects...");

  do
  {
    if (SERIAL_BT.connected()) {
      DBG("Bluetooth connected. Switching to brain mode.");
      return false;
    }

#ifdef ENABLE_DISPLAY_PIN
    if(digitalRead(PIN_DISPLAY_ENABLE_IN)) {
      DBG("Display connected. Switching to passthrough mode.");
      return true;
    }
#endif

    current_time = millis();
  }
  while((current_time - start_time) <= PASSTHROUGH_MODE_WAIT);

  DBG("Nothing did not connect. Switching to passthrough mode.");
  return true;
}

void setup()
{
  pinMode(PIN_MOTOR_ENABLE_OUT, OUTPUT);
#ifdef ENABLE_DISPLAY_PIN
  pinMode(PIN_DISPLAY_ENABLE_IN, INPUT_PULLDOWN);
#endif

  SERIAL_MOTOR.begin(EBIKE_COMM_BAUDRATE);
  SERIAL_DISPLAY.begin(EBIKE_COMM_BAUDRATE);
  SERIAL_BT.begin(BT_NAME);

  set_motor(false);

#if defined(DEBUGGING)
  while(!SERIAL_BT.connected());
  DBG("[DEBUGGING ENABLED]");
  DBG("Waiting 5 seconds before continuing... (disconnect now if you want passthrough mode)");
  sleep(5);
#endif

  is_passthrough_mode = wait_for_passthrough();
}

#ifdef ENABLE_LOGGING_PASSTHROUGH

void log_passthrough_oneway(Stream* from, Stream* to, const char* from_name, const char* to_name, Stream* log, uint8_t state)
{
  static uint8_t last_state = 255;
  if(from->available())
  {
    auto b = from->read();
    to->write(b);

    if(last_state != state)
    {
      if(last_state != 255)
        log->println();
      log->printf("%s -> %s:", from_name, to_name);
      last_state = state;
    }

    log->printf(" %02X", b);
  }
}

void log_passthrough(Stream* stream1, Stream* stream2, Stream* log)
{
  log_passthrough_oneway(stream1, stream2, "MOTOR", "DISPLAY", &SERIAL_BT, 0);
  log_passthrough_oneway(stream2, stream1, "DISPLAY", "MOTOR", &SERIAL_BT, 1);
}

#endif

void passthrough_oneway(Stream* from, Stream* to)
{
  if(from->available())
  {
    auto b = from->read();
    to->write(b);
  }
}

void passthrough(Stream* stream1, Stream* stream2)
{
  passthrough_oneway(stream1, stream2);
  passthrough_oneway(stream2, stream1);
}

void loop_passthrough()
{
#ifdef ENABLE_DISPLAY_PIN
  set_motor(digitalRead(PIN_DISPLAY_ENABLE_IN));
#else
  set_motor(true);
#endif

#ifdef ENABLE_LOGGING_PASSTHROUGH
  log_passthrough(&SERIAL_MOTOR, &SERIAL_DISPLAY, &SERIAL_BT);
#else
  passthrough(&SERIAL_MOTOR, &SERIAL_DISPLAY);
#endif
}

void loop_brain()
{
  set_motor(false);
  passthrough(&SERIAL_MOTOR, &SERIAL_BT);
}

void loop()
{
  if (is_passthrough_mode) {
    loop_passthrough();
  } else {
    loop_brain();
  }
}
