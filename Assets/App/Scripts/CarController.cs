using System.Collections;
using TofAr.V0.Body;
using TofAr.V0.Hand;
using UnityEngine;
using UnityEngine.UI;

namespace TofArGestureDrive
{
    public class CarController : MonoBehaviour
    {
        public Rigidbody RTire;
        public Rigidbody LTire;
        public float fireInterval = 0.1f;
        public HandModel RHand;
        public HandModel LHand;
        public Text RPowerText;
        public Text LPowerText;
        public float maxDistance = 0.5f;
        public float forceFactor = 50f;
        public float debugForceR = 0f;
        public float debugForceL = 0f;

        private void Start()
        {
            this.StartCoroutine(this.Process());
        }

        private IEnumerator Process()
        {
            var useFrontCamera = TofArHandManager.Instance.RecogMode == RecogMode.Face2Face;
            while (true)
            {
                // カメラから手の中心までの距離を推進力に変換 0(m)->100% maxDistance(m)m->0%
                if (useFrontCamera)
                {
                    // Frontカメラの場合、手の左右は逆転する
                    var rPower = 0f;
                    if ((this.LHand.handStatus == HandStatus.BothHands) && (this.LHand.HandPoints?.Length > 0))
                    {
                        var handDistance = this.LHand.HandPoints[(int)HandPointIndex.HandCenter].z;
                        rPower = Mathf.Max(0, this.maxDistance - handDistance);
                    }
                    this.RTire.AddForce(this.RTire.transform.forward * rPower * this.forceFactor);
                    this.ShowAccelePower(this.RPowerText, rPower);


                    var lPower = 0f;
                    if ((this.RHand.handStatus == HandStatus.BothHands) && (this.RHand.HandPoints?.Length > 0))
                    {
                        var handDistance = this.RHand.HandPoints[(int)HandPointIndex.HandCenter].z;
                        lPower = Mathf.Max(0, this.maxDistance - handDistance);
                    }
                    this.LTire.AddForce(this.LTire.transform.forward * lPower * this.forceFactor);
                    this.ShowAccelePower(this.LPowerText, lPower);
                }
                else
                {
                    var rPower = 0f;
                    if ((this.LHand.handStatus == HandStatus.BothHands) && (this.LHand.HandPoints?.Length > 0))
                    {
                        var handDistance = this.LHand.HandPoints[(int)HandPointIndex.HandCenter].z;
                        rPower = Mathf.Min(this.maxDistance, handDistance);
                    }
                    this.LTire.AddForce(this.LTire.transform.forward * rPower * this.forceFactor);
                    this.ShowAccelePower(this.LPowerText, rPower);


                    var lPower = 0f;
                    if ((this.RHand.handStatus == HandStatus.BothHands) && (this.RHand.HandPoints?.Length > 0))
                    {
                        var handDistance = this.RHand.HandPoints[(int)HandPointIndex.HandCenter].z;
                        lPower = Mathf.Min(this.maxDistance, handDistance);
                    }
                    this.RTire.AddForce(this.RTire.transform.forward * lPower * this.forceFactor);
                    this.ShowAccelePower(this.RPowerText, lPower);
                }

                // for debug
                this.RTire.AddForce(this.RTire.transform.forward * this.debugForceR);
                this.LTire.AddForce(this.LTire.transform.forward * this.debugForceL);

                yield return new WaitForSeconds(this.fireInterval);
            }
        }

        private void ShowAccelePower(Text text, float power)
        {
            power = Mathf.Floor((power / this.maxDistance) * 100f);
            var guageText = $"{power}%";
            for (var row = 9; row >= 0; row--)
            {
                guageText += Mathf.Floor(power / 10) > row ? "\n■■■" : "\n□□□";
            }
            text.text = guageText;
        }
    }

}