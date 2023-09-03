using UnityEngine;

namespace NDS.BulletSystem.Parabolic
{
    public class ParabolicBullet : MonoBehaviour
    {
        #region Variables
        private float speed;
        private float gravity;
        private float Spread;
        private Transform startTransform;
        private Vector3 startPosition;
        private Vector3 startForward;

        private bool isInitialized = false;

        private float startTime = -1;
        #endregion

        #region Initialization
        public void Initialize(Transform startPoint, float speed, float gravity, float Spread)
        {
            this.startTransform = startPoint;
            this.startForward = startPoint.forward.normalized;
            this.speed = speed;
            this.gravity = gravity;
            this.Spread = Spread;
            isInitialized = true;
        }
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            //Spread
            float x = Random.Range(-Spread, Spread);
            float y = Random.Range(-Spread, Spread);

            startPosition = startTransform.position + new Vector3(x, y, 0f);
        }

        private void FixedUpdate()
        {

            if (!isInitialized) return;
            if (startTime < 0) startTime = Time.time;

            float currentTime = Time.time - startTime;
            float prevTime = currentTime - Time.fixedDeltaTime;
            float nextTime = currentTime + Time.fixedDeltaTime;

            RaycastHit hit;
            Vector3 currentPoint = FindPointOnParabola(currentTime);

            if (prevTime > 0)
            {
                Vector3 prevPoint = FindPointOnParabola(prevTime);
                if (CastRayBetweenPoints(prevPoint, currentPoint, out hit))
                {
                    OnHit(hit);
                }
            }

            Vector3 nextPoint = FindPointOnParabola(nextTime);
            if (CastRayBetweenPoints(currentPoint, nextPoint, out hit))
            {
                OnHit(hit);
            }
        }

        private void Update()
        {
            if (!isInitialized || startTime < 0) return;

            float currentTime = Time.time - startTime;
            Vector3 currentPoint = FindPointOnParabola(currentTime);
            transform.position = currentPoint;
        }

        #endregion

        #region Private Methods
        private Vector3 FindPointOnParabola(float time)
        {
            Vector3 point = startPosition + (startForward * time * speed);
            Vector3 gravityVec = Vector3.down * time * time * gravity;
            return point + gravityVec;
        }

        private bool CastRayBetweenPoints(Vector3 startPoint, Vector3 endPoint, out RaycastHit hit)
        {
            Debug.DrawRay(startPoint, endPoint - startPoint, Color.green, 5);
            return Physics.Raycast(startPoint, endPoint - startPoint, out hit, (endPoint - startPoint).magnitude);
        }

        private void OnHit(RaycastHit hit)
        {
            ShootableObject shootableObject = hit.transform.GetComponent<ShootableObject>();
            if (shootableObject)
            {
                shootableObject.OnHit(hit);
            }
            Destroy(gameObject);
        }
        #endregion
    }
}

