using System;
using System.Text;
using UnityEngine;
using TMPro;

namespace UI
{
    public enum ASCIShape
    {
        Cube,
        Donut
    }

    public class DonutRenderer : MonoBehaviour
    {
        [SerializeField] private TMP_Text asciiDisplayText;

        [Header("Screen Settings")]
        [Tooltip("Width of ASCII display in characters.")]
        [SerializeField] private int asciiScreenWidth = 160;

        [Tooltip("Height of ASCII display in characters.")]
        [SerializeField] private int asciiScreenHeight = 44;

        [Header("Projection Settings")]
        [Tooltip("Controls how large the object appears on screen.")]
        [SerializeField] private float projectionScale = 50f;

        [Tooltip("Simulates distance from camera to the object.")]
        [SerializeField] private float cameraDistance = 100f;
        
        [Tooltip("Direction of the simulated light source (e.g., (0,1,-1)).")]
        [SerializeField] private Vector3 lightDirectionVector = new Vector3(0, 1, -1);

        [Header("Rotation Settings")]
        [Tooltip("Speed of rotation around X-axis per frame.")]
        [SerializeField] private float rotationSpeedX = 0.02f;

        [Tooltip("Speed of rotation around Y-axis per frame.")]
        [SerializeField] private float rotationSpeedY = 0.05f;

        [Tooltip("Speed of rotation around Z-axis per frame.")]
        [SerializeField] private float rotationSpeedZ = 0.01f;

        [Header("Cube Settings")]
        [Tooltip("Half-size of the cube (so total width is double this).")]
        [SerializeField] private float cubeHalfSize = 20f;

        [Tooltip("Step size for drawing cube faces (smaller = smoother, but slower).")]
        [SerializeField] private float cubeDetailStep = 0.2f;

        [Header("Donut Settings")]
        [Tooltip("Major radius of the donut (distance from center to tube center).")]
        [SerializeField] private float donutMajorRadius = 10f;

        [Tooltip("Minor radius of the donut (radius of the tube itself).")]
        [SerializeField] private float donutMinorRadius = 5f;

        [Tooltip("Step size for drawing the donut curves (smaller = smoother, but slower).")]
        [SerializeField] private float donutDetailStep = 0.4f;

        [Header("Shape Selection")]
        [Tooltip("Choose which shape to render: Cube or Donut.")]
        [SerializeField] private ASCIShape selectedShape = ASCIShape.Cube;

        private string[] output;
        private float[] zBuffer;
        private float rotationX = 1.0f;
        private float rotationY = 0.5f;
        private float rotationZ;
        private const string LUMINANCE_CHARS = ".~!*=#";


        private void Start()
        {
            output = new string[asciiScreenWidth * asciiScreenHeight];
            zBuffer = new float[asciiScreenWidth * asciiScreenHeight];
        }
        

        private void FixedUpdate()
        {
            Array.Fill(output, " ");
            Array.Fill(zBuffer, 0f);

            if (selectedShape == ASCIShape.Cube)
                DrawCube();
            else if (selectedShape == ASCIShape.Donut)
                DrawDonut();

            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < asciiScreenWidth * asciiScreenHeight; ++k)
            {
                if (k % asciiScreenWidth == 0)
                    sb.Append('\n');
                sb.Append(output[k]);
            }

            asciiDisplayText.text = sb.ToString();

            Rotate();
        }

        private void DrawCube()
        {
            for (float a = -cubeHalfSize; a <= cubeHalfSize; a += cubeDetailStep)
            {
                for (float b = -cubeHalfSize; b <= cubeHalfSize; b += cubeDetailStep)
                {
                    PlotPlane(a, b, cubeHalfSize, 0, 0, 1); // front
                    PlotPlane(a, b, -cubeHalfSize, 0, 0, -1); // back
                    PlotPlane(cubeHalfSize, a, b, 1, 0, 0); // right
                    PlotPlane(-cubeHalfSize, a, b, -1, 0, 0); // left
                    PlotPlane(a, cubeHalfSize, b, 0, 1, 0); // top
                    PlotPlane(a, -cubeHalfSize, b, 0, -1, 0); // bottom
                }
            }
        }

        private void DrawDonut()
        {
            float R = donutMajorRadius;
            float r = donutMinorRadius;
            float step = donutDetailStep;

            for (float theta = 0; theta < 2 * Mathf.PI; theta += step)
            {
                float costheta = Mathf.Cos(theta);
                float sintheta = Mathf.Sin(theta);

                for (float phi = 0; phi < 2 * Mathf.PI; phi += step)
                {
                    float cosphi = Mathf.Cos(phi);
                    float sinphi = Mathf.Sin(phi);

                    float x = (R + r * cosphi) * costheta;
                    float y = (R + r * cosphi) * sintheta;
                    float z = r * sinphi;

                    float nx = cosphi * costheta;
                    float ny = cosphi * sintheta;
                    float nz = sinphi;

                    PlotPoint(x, y, z, nx, ny, nz);
                }
            }
        }


        private void PlotPoint(float x, float y, float z, float nx, float ny, float nz)
        {
            RotatePoint(ref x, ref y, ref z);
            RotateVector(ref nx, ref ny, ref nz);

            float ooz = 1 / (z + cameraDistance);
            int xp = (int)(asciiScreenWidth / 2 + projectionScale * ooz * x * 2);
            int yp = (int)(asciiScreenHeight / 2 - projectionScale * ooz * y);
            int pos = xp + yp * asciiScreenWidth;

            float L = nx * lightDirectionVector.x + ny * lightDirectionVector.y + nz * lightDirectionVector.z;
            int luminanceIndex = Mathf.Clamp(Mathf.FloorToInt(L * (LUMINANCE_CHARS.Length - 1)), 0,
                LUMINANCE_CHARS.Length - 1);

            if (ooz > zBuffer[pos])
            {
                zBuffer[pos] = ooz;
                output[pos] = LUMINANCE_CHARS[luminanceIndex].ToString();
            }
        }


        private void PlotPlane(float x, float y, float z, float nx, float ny, float nz)
        {
            RotatePoint(ref x, ref y, ref z);
            RotateVector(ref nx, ref ny, ref nz);

            float ooz = 1 / (z + cameraDistance);
            int xp = (int)(asciiScreenWidth / 2 + projectionScale * ooz * x * 2);
            int yp = (int)(asciiScreenHeight / 2 - projectionScale * ooz * y);
            int pos = xp + yp * asciiScreenWidth;

            float L = nx * lightDirectionVector.x + ny * lightDirectionVector.y + nz * lightDirectionVector.z;
            int luminanceIndex = Mathf.Clamp(Mathf.FloorToInt(L * (LUMINANCE_CHARS.Length - 1)), 0,
                LUMINANCE_CHARS.Length - 1);

            if (ooz > zBuffer[pos])
            {
                zBuffer[pos] = ooz;
                output[pos] = LUMINANCE_CHARS[luminanceIndex].ToString();
            }
        }


        private void RotatePoint(ref float x, ref float y, ref float z)
        {
            float cosA = Mathf.Cos(rotationX), sinA = Mathf.Sin(rotationX);
            float cosB = Mathf.Cos(rotationY), sinB = Mathf.Sin(rotationY);
            float cosC = Mathf.Cos(rotationZ), sinC = Mathf.Sin(rotationZ);

            float rotatedX = y * sinA * sinB * cosC - z * cosA * sinB * cosC + y * cosA * sinC + z * sinA * sinC +
                             x * cosB * cosC;
            float rotatedY = y * cosA * cosC + z * sinA * cosC - y * sinA * sinB * sinC + z * cosA * sinB * sinC -
                             x * cosB * sinC;
            float rotatedZ = z * cosA * cosB - y * sinA * cosB + x * sinB;

            x = rotatedX;
            y = rotatedY;
            z = rotatedZ;
        }


        private void RotateVector(ref float x, ref float y, ref float z)
        {
            float cosA = Mathf.Cos(rotationX), sinA = Mathf.Sin(rotationX);
            float cosB = Mathf.Cos(rotationY), sinB = Mathf.Sin(rotationY);
            float cosC = Mathf.Cos(rotationZ), sinC = Mathf.Sin(rotationZ);

            float rotatedX = y * sinA * sinB * cosC - z * cosA * sinB * cosC + y * cosA * sinC + z * sinA * sinC +
                             x * cosB * cosC;
            float rotatedY = y * cosA * cosC + z * sinA * cosC - y * sinA * sinB * sinC + z * cosA * sinB * sinC -
                             x * cosB * sinC;
            float rotatedZ = z * cosA * cosB - y * sinA * cosB + x * sinB;

            x = rotatedX;
            y = rotatedY;
            z = rotatedZ;
        }


        private void Rotate()
        {
            rotationX += rotationSpeedX;
            rotationY += rotationSpeedY;
            rotationZ += rotationSpeedZ;
        }
    }
}