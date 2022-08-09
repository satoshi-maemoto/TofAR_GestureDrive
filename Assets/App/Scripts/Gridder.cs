using UnityEngine;

namespace TofArGestureDrive
{
    public class Gridder : MonoBehaviour
    {
        public GameObject gridLinePrefab;
        public GameObject floor;
        public float gridInterval = 1f;

        private const float PlaneScale = 10f;

        void Start()
        {
            var maxX = Mathf.Floor(this.floor.transform.localScale.x * PlaneScale / 2);
            for (var x = maxX; x >= 0; x -= this.gridInterval)
            {
                this.CreateGridLine(new Vector3(x, 0f, 0f), Quaternion.Euler(90, 0, 0), maxX);
                this.CreateGridLine(new Vector3(-x, 0f, 0f), Quaternion.Euler(90, 0, 0), maxX);
            }
            var maxZ = Mathf.Floor(this.floor.transform.localScale.z * PlaneScale / 2);
            for (var z = maxZ; z >= 0; z -= this.gridInterval)
            {
                this.CreateGridLine(new Vector3(0f, 0f, z), Quaternion.Euler(0, 0, 90), maxZ);
                this.CreateGridLine(new Vector3(0f, 0f, -z), Quaternion.Euler(0, 0, 90), maxZ);
            }
        }

        private void CreateGridLine(Vector3 position, Quaternion rotation, float yScale)
        {
            var grid = GameObject.Instantiate(this.gridLinePrefab, position, rotation);
            grid.transform.localScale = new Vector3(
                grid.transform.localScale.x,
                yScale,
                grid.transform.localScale.z);
            grid.transform.parent = this.floor.transform;
        }
    }
}