using System.Collections;
using TofAr.V0.Hand;
using UnityEngine;

namespace TofArGestureDrive
{
    public class CarController : MonoBehaviour
    {
        public Rigidbody RTire;
        public Rigidbody LTire;
        public float fireInterval = 0.1f;
        public HandModel RHand;
        public HandModel LHand;
        public float forceFactor = 50f;
        public float debugForceR = 0f;
        public float debugForceL = 0f;

        private void Start()
        {
            this.StartCoroutine(this.Process());
        }

        private IEnumerator Process()
        {
            while (true)
            {
                if (((this.RHand.handStatus == HandStatus.RightHand) || (this.RHand.handStatus == HandStatus.BothHands)) &&
                    this.RHand.HandPoints != null && this.RHand.HandPoints.Length > 0)
                {
                    var accele = this.RHand.HandPoints[(int)HandPointIndex.HandCenter].z;
                    Debug.Log($"Rz:{accele}");
                    var force = 0.5f - accele;
                    this.RTire.AddForce(this.RTire.transform.forward * force * this.forceFactor);
                }
                if (((this.LHand.handStatus == HandStatus.LeftHand) || (this.RHand.handStatus == HandStatus.BothHands)) &&
                    this.LHand.HandPoints != null && this.LHand.HandPoints.Length > 0)
                {
                    var accele = this.LHand.HandPoints[(int)HandPointIndex.HandCenter].z;
                    Debug.Log($"Rz:{accele}");
                    var force = 0.5f - accele;
                    this.LTire.AddForce(this.LTire.transform.forward * force * this.forceFactor);
                }

                this.RTire.AddForce(this.RTire.transform.forward * this.debugForceR);
                this.LTire.AddForce(this.LTire.transform.forward * this.debugForceL);

                yield return new WaitForSeconds(this.fireInterval);
            }
        }
    }

}