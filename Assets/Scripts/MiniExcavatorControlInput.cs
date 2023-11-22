using UnityEngine;

namespace VehiclePhysics.Specialized
{

    public class MiniExcavatorControlInput : VehicleBehaviour
    {
        [Range(0.01f, 2.0f)]
        public float movementRate = 1.0f;

        [Space(5)]
        public KeyCode swingLeft = KeyCode.Keypad0;
        public KeyCode swingRight = KeyCode.KeypadPeriod;
        public KeyCode kingpostLeft = KeyCode.Keypad1;
        public KeyCode kingpostRight = KeyCode.Keypad3;
        public KeyCode boomUp = KeyCode.Keypad2;
        public KeyCode boomDown = KeyCode.Keypad5;
        public KeyCode stickUp = KeyCode.Keypad8;
        public KeyCode stickDown = KeyCode.KeypadDivide;
        public KeyCode bucketUp = KeyCode.Keypad9;
        public KeyCode bucketDown = KeyCode.Keypad7;

        MiniExcavatorControl m_excavator;


        public override int GetUpdateOrder()
        {
            // Should update before ExcavatorControl
            return -1;
        }


        public override void OnEnableVehicle()
        {
            m_excavator = vehicle.GetComponentInChildren<MiniExcavatorControl>();
        }


        public override void OnDisableVehicle()
        {
            m_excavator.swingInput = 0.0f;
            m_excavator.kingpostInput = 0.0f;
            m_excavator.boomInput = 0.0f;
            m_excavator.stickInput = 0.0f;
            m_excavator.bucketInput = 0.0f;
        }


        public override void FixedUpdateVehicle()
        {

            float swing = 0.0f;
            if (Input.GetKey(swingLeft)) swing -= 1.0f;
            if (Input.GetKey(swingRight)) swing += 1.0f;

            float kingpost = 0.0f;
            if (Input.GetKey(kingpostLeft)) kingpost -= 1.0f;
            if (Input.GetKey(kingpostRight)) kingpost += 1.0f;

            float boom = 0.0f;
            if (Input.GetKey(boomUp)) boom += 1.0f;
            if (Input.GetKey(boomDown)) boom -= 1.0f;

            float stick = 0.0f;
            if (Input.GetKey(stickUp)) stick += 1.0f;
            if (Input.GetKey(stickDown)) stick -= 1.0f;

            float bucket = 0.0f;
            if (Input.GetKey(bucketUp)) bucket += 1.0f;
            if (Input.GetKey(bucketDown)) bucket -= 1.0f;

            m_excavator.swingInput = swing * movementRate;
            m_excavator.kingpostInput = kingpost * movementRate;
            m_excavator.boomInput = boom * movementRate;
            m_excavator.stickInput = stick * movementRate;
            m_excavator.bucketInput = bucket * movementRate;
        }
    }

}
