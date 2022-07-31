using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace sluggagames.jumper.Data.Helpers
{
    public class GetRandomPoint : MonoBehaviour
    {
        public static GetRandomPoint Instance;
        public float Range;
        public int numberOfPoints;


        private void Awake()
        {
            //todo make into a singleton from the base singleton
            Instance = this;
        }
        bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {

            for (int i = 0; i < numberOfPoints; i++)
            {
                Vector3 randomPoint = center + Random.insideUnitSphere * range;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }

            }
            result = Vector3.zero;
            return false;
        }

        public Vector3 GetPoint(Transform point = null, float radius = 0)
        {
            Vector3 _point;
            if (RandomPoint(point == null ? transform.position : point.position, radius == 0 ? Range : radius, out _point))
            {
                Debug.DrawRay(_point, Vector3.up, Color.black, 1);
                return _point;
            }

            return point == null ? Vector3.zero : point.position;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Range);
        }
#endif
    }
}
