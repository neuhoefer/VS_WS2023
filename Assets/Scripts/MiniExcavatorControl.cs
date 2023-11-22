using UnityEngine;

namespace VehiclePhysics.Specialized
{

    public class MiniExcavatorControl : VehicleBehaviour
    {
        public VPVehicleJoint swingJoint;
        public Transform swingTransform;

        public VPVehicleJoint kingpostJoint;
        public Transform kingpostTransform;

        public VPVehicleJoint boomJoint;
        public Transform boomTransform;

        public VPVehicleJoint stickJoint;
        public Transform stickTransform;

        public VPVehicleJoint bucketJoint;
        public Transform bucketTransform;

        [Space(5)]
        public bool swingLimit = false;
        public float minSwingAngle = -90.0f;
        public float maxSwingAngle = 90.0f;

        public float minKingpostAngle = 0.0f;
        public float maxKingpostAngle = 100.0f;

        public float minBoomAngle = 0.0f;
        public float maxBoomAngle = 100.0f;

        public float minStickAngle = 0.0f;
        public float maxStickAngle = 100.0f;

        public float minBucketAngle = 0.0f;
        public float maxBucketAngle = 150.0f;

        [Space(5)]
        public float swingRate = 0.1f;
        public float kingpostRate = 0.5f;
        public float boomRate = 0.5f;
        public float stickRate = 0.5f;
        public float bucketRate = 0.5f;

        [Space(5)]
        [Range(0, 1)]
        public float startSwingPosition = 0.0f;
        [Range(0, 1)]
        public float startKingpostPosition = 0.5f;
        [Range(0, 1)]
        public float startBoomPosition = 0.5f;
        [Range(0, 1)]
        public float startStickPosition = 0.5f;
        [Range(0, 1)]
        public float startBucketPosition = 0.5f;

        [Space(5)]
        [Range(-1, 1)]
        public float swingInput = 0.0f;
        [Range(-1, 1)]
        public float kingpostInput = 0.0f;
        [Range(-1, 1)]
        public float boomInput = 0.0f;
        [Range(-1, 1)]
        public float stickInput = 0.0f;
        [Range(-1, 1)]
        public float bucketInput = 0.0f;

        [Space(5)]
        public bool showDebugLabels = false;

        // Current positions exposed
        //
        // Note: these are the expected positions, but not necessarily the actual positions.
        // (i.e. parts might be blocked by other objects, or forced under heavy load)

        public float swingPosition { get { return m_swingPosition; } }
        public float kingpostPosition { get { return m_kingpostPosition; } }
        public float boomPosition { get { return m_boomPosition; } }
        public float stickPosition { get { return m_stickPosition; } }
        public float bucketPosition { get { return m_bucketPosition; } }


        // Private fields

        float m_swingPosition = 0.0f;
        float m_kingpostPosition = 0.0f;
        float m_boomPosition = 0.0f;
        float m_stickPosition = 0.0f;
        float m_bucketPosition = 0.0f;
        bool m_firstRun;


        public override void OnEnableVehicle()
        {
            // Initial position

            m_swingPosition = startSwingPosition;
            m_kingpostPosition = startKingpostPosition;
            m_boomPosition = startBoomPosition;
            m_stickPosition = startStickPosition;
            m_bucketPosition = startBucketPosition;

            // First run after enable resets the positions.
            // This prevents the rig to apply inertias when setting the startup positions.

            m_firstRun = true;
        }


        public override void FixedUpdateVehicle()
        {
            // Apply inputs to positions

            if (swingLimit)
                m_swingPosition = Mathf.Clamp01(m_swingPosition + swingInput * swingRate * Time.deltaTime);
            else
                m_swingPosition = m_swingPosition + swingInput * swingRate * Time.deltaTime;

            m_kingpostPosition = Mathf.Clamp01(m_kingpostPosition + kingpostInput * kingpostRate * Time.deltaTime);
            m_boomPosition = Mathf.Clamp01(m_boomPosition + boomInput * boomRate * Time.deltaTime);
            m_stickPosition = Mathf.Clamp01(m_stickPosition + stickInput * stickRate * Time.deltaTime);
            m_bucketPosition = Mathf.Clamp01(m_bucketPosition + bucketInput * bucketRate * Time.deltaTime);

            // Update swing joint around the Y axis

            if (swingJoint != null)
            {
                float swingAngle = swingLimit ? Mathf.Lerp(minSwingAngle, maxSwingAngle, m_swingPosition) : 360.0f * m_swingPosition;
                swingJoint.targetRotation = Quaternion.AngleAxis(-swingAngle, Vector3.up) * swingJoint.referenceRotation;

                // First run translates the rotation to the current position

                if (m_firstRun)
                    swingJoint.transform.localRotation = Quaternion.AngleAxis(swingAngle, Vector3.up);
            }

            // Update attachment joints around their X axes

            UpdateAttachmentJoint(kingpostJoint, minKingpostAngle, maxKingpostAngle, m_kingpostPosition);
            UpdateAttachmentJoint(boomJoint, minBoomAngle, maxBoomAngle, m_boomPosition);
            UpdateAttachmentJoint(stickJoint, minStickAngle, maxStickAngle, m_stickPosition);
            UpdateAttachmentJoint(bucketJoint, minBucketAngle, maxBucketAngle, m_bucketPosition);

            // First run completed

            m_firstRun = false;
        }


        void LateUpdate()
        {
            // Rotational joints update the visual rotation in local coordinates

            UpdateAttachmentTransform(swingTransform, swingJoint);
            UpdateAttachmentTransform(kingpostTransform, kingpostJoint);
            UpdateAttachmentTransform(boomTransform, boomJoint);
            UpdateAttachmentTransform(stickTransform, stickJoint);
            UpdateAttachmentTransform(bucketTransform, bucketJoint);
        }


        void UpdateAttachmentJoint(VPVehicleJoint joint, float minAngle, float maxAngle, float position)
        {
            if (joint != null)
            {
                float angle = Mathf.Lerp(minAngle, maxAngle, position);
                joint.targetRotation = Quaternion.AngleAxis(-angle, Vector3.right) * joint.referenceRotation;

                if (m_firstRun)
                    joint.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.right);
            }
        }


        void UpdateAttachmentTransform(Transform trans, VPVehicleJoint joint)
        {
            if (joint != null && trans != null)
                trans.localRotation = joint.transform.localRotation;
        }


#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (isActiveAndEnabled && showDebugLabels && Application.isPlaying)
            {
                if (swingLimit)
                    ShowLabel(swingJoint, "Swing", minSwingAngle, maxSwingAngle, m_swingPosition);
                else
                    ShowLabelUnconstrained(swingJoint, "Swing", m_swingPosition);

                ShowLabel(kingpostJoint, "Kingpost", minKingpostAngle, maxKingpostAngle, m_kingpostPosition);
                ShowLabel(boomJoint, "Boom", minBoomAngle, maxBoomAngle, m_boomPosition);
                ShowLabel(stickJoint, "Stick", minStickAngle, maxStickAngle, m_stickPosition);
                ShowLabel(bucketJoint, "Bucket", minBucketAngle, maxBucketAngle, m_bucketPosition);
            }
        }

        // Public static methods so they can be used by other joint-based control scripts

        public static void ShowLabel(VPVehicleJoint joint, string caption, float minAngle, float maxAngle, float position)
        {
            if (joint != null)
            {
                Transform trans = joint.thisAnchor;
                if (trans != null)
                {
                    float angle = Mathf.Lerp(minAngle, maxAngle, position);
                    if (angle < -180.0f) angle += 360.0f;
                    if (angle > 180.0f) angle -= 360.0f;
                    UnityEditor.Handles.Label(trans.position, caption + ": " + angle.ToString("0.0") + " [" + position.ToString("0.0") + "]");
                }
            }
        }

        public static void ShowLabelUnconstrained(VPVehicleJoint joint, string caption, float position)
        {
            if (joint != null)
            {
                Transform trans = joint.thisAnchor;
                if (trans != null)
                {
                    float angle = 360.0f * position;
                    UnityEditor.Handles.Label(trans.position, caption + ": " + angle.ToString("0.0") + " [" + position.ToString("0.0") + "]");
                }
            }
        }

        public static void ShowLabelSymmetric(VPVehicleJoint joint, string caption, float maxAngle, float position)
        {
            if (joint != null)
            {
                Transform trans = joint.thisAnchor;
                if (trans != null)
                {
                    float angle = position * maxAngle;
                    if (angle < -180.0f) angle += 360.0f;
                    if (angle > 180.0f) angle -= 360.0f;
                    UnityEditor.Handles.Label(trans.position, caption + ": " + angle.ToString("0.0") + " [" + position.ToString("0.0") + "]");
                }
            }
        }

#endif
    }

}
