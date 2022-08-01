using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace sluggagames.jumper.Enemies
{
    public class EnemyVisibility : MonoBehaviour
    {
        [SerializeField]
        private Transform target = null;
        [SerializeField]
        internal float maxDistance = 10f;
        [Range(0f, 180)]
        [SerializeField]
        internal float angle = 45f;

        [SerializeField]
        private bool visualize = true;


        private void Start()
        {
            try
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            catch (UnityException ex)
            {
                Debug.LogError(ex.Message);
                return;
            }
        }

        public bool CheckVisibilityOriginal()
        {
            var directionToTarget = target.position - GetComponentInParent<EnemyController>().transform.position;
            var degreesToTarget = Vector3.Angle(transform.forward, directionToTarget);
            var withinArc = degreesToTarget < (angle / 2);
            if (!withinArc)
            {
                return false;
            }
            var distanceToTarget = directionToTarget.magnitude;
            var rayDistance = Mathf.Min(maxDistance, distanceToTarget);
            var ray = new Ray(transform.position, directionToTarget);

            RaycastHit hit;

            var canSee = false;

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.collider.transform == target)
                {
                    canSee = true;
                }

                Debug.DrawLine(transform.position, hit.point);

            }
            else
            {
                Debug.DrawRay(transform.position, directionToTarget.normalized * rayDistance);
            }
            return canSee;
        }


        public bool CheckVisibilityToPoint(Vector3 worldPoint)
        {
            Vector3 directionToTarget;
            float degreesToTarget;
            GetDirectionAndAngleToTarget(worldPoint, out directionToTarget, out degreesToTarget);
            if (!IsWithinArc(worldPoint, directionToTarget))
            {
                return false;
            }
            return CheckVisibilityRay(directionToTarget, target);
        }

        public bool CheckVisibilityOfTarget()
        {
            Vector3 directionToTarget;
            float degreesToTarget;

            GetDirectionAndAngleToTarget(target.position, out directionToTarget, out degreesToTarget);
            if (!IsWithinArc(target.position, directionToTarget))
            {
                return false;
            }
            return CheckVisibilityRay(directionToTarget, target);



        }

        private void GetDirectionAndAngleToTarget(Vector3 point, out Vector3 directionToTarget, out float degreesToTarget)
        {
            directionToTarget = point - transform.position;
            degreesToTarget = Vector3.Angle(transform.forward, directionToTarget);

        }
        private bool IsWithinArc(Vector3 targetPoint, Vector3 direction)
        {
            bool within = false;
            float degreesToTarget = Vector3.Angle(transform.forward, direction);


            bool withinArc = degreesToTarget < (angle / 2);
            if (withinArc)
            {
                within = true;
            }
            return within;
        }
        private bool CheckVisibilityRay(Vector3 direction, Transform _target)
        {
            float distanceToTarget = direction.magnitude;
            float rayDistance = Mathf.Min(maxDistance, distanceToTarget);

            Ray ray = new Ray(transform.position, direction);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.collider.transform == _target)
                {
                    return true; // point is target, we see it so true.
                }
                return false; // obstructed view

            }
            else
            {
                return true; // can see targeted point
            }
        }


    }

#if UNITY_EDITOR
    [CustomEditor(typeof(EnemyVisibility))]
    public class EnemyVisibilityEditor : Editor
    {

        private void OnSceneGUI()
        {
            EnemyVisibility visibility = target as EnemyVisibility;

            Handles.color = new Color(1, 1, 1, 0.5f);

            Vector3 forwardPointMinusHalfAngle = Quaternion.Euler(0, -visibility.angle / 2, 0) * visibility.transform.forward;

            Vector3 arcStart = forwardPointMinusHalfAngle * visibility.maxDistance;

            Handles.DrawSolidArc(visibility.transform.position, Vector3.up, arcStart, visibility.angle, visibility.maxDistance);
            Handles.color = Color.white;
            Vector3 handlePosition = visibility.transform.position + visibility.transform.forward * visibility.maxDistance;

            visibility.maxDistance = Handles.ScaleValueHandle(visibility.maxDistance, handlePosition, visibility.transform.rotation, 1, Handles.ConeHandleCap, 0.25f);
        }
    }
#endif
}
