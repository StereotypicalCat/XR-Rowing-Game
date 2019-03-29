using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterUpdater : MonoBehaviour
{
        public float waveHeight = 0.5f;
        public float waveFrequency = 0.5f;
        public float waveLength = 0.75f;

        public int interval = 2;
        public int frameIntervalToWorkOn = 0;

        public float currentTime = 0;
        
        public GameObject[] waters = new GameObject[3];
        
        //Position where the waves originate from
        public Vector3 waveOriginPosition = new Vector3(0.0f, 0.0f, 0.0f);

        public MeshFilter[] watersMeshFilters = new MeshFilter[3];
        public Mesh[] watersMeshes = new Mesh[3];
        public Vector3[] vertices; 

        private void Awake()
        {
            for (int i = 0; i < waters.Length; i++)
            {
                var meshFilter = waters[i].GetComponent<MeshFilter>();
                watersMeshFilters[i] = meshFilter;
                watersMeshes[i] = meshFilter.mesh;

            }
        }

        void Start()
        {
            for (int i = 0; i < waters.Length; i++)
            {
                CreateMeshLowPoly(watersMeshFilters[i], i);
            }

            vertices = watersMeshes[0].vertices;
            currentTime = Time.time;
           
        }

        /// <summary>
        /// Rearranges the mesh vertices to create a 'low poly' effect
        /// </summary>
        /// <param name="mf">Mesh filter of gamobject</param>
        /// <returns></returns>
        void CreateMeshLowPoly(MeshFilter mf, int index)
        {
            watersMeshes[index] = mf.sharedMesh;

            //Get the original vertices of the gameobject's mesh
            Vector3[] originalVertices = watersMeshes[index].vertices;

            //Get the list of triangle indices of the gameobject's mesh
            int[] triangles = watersMeshes[index].triangles;

            //Create a vector array for new vertices 
            Vector3[] vertices = new Vector3[triangles.Length];
            
            //Assign vertices to create triangles out of the mesh
            for (int i = 0; i < triangles.Length; i++)
            {
                vertices[i] = originalVertices[triangles[i]];
                triangles[i] = i;
            }

            //Update the gameobject's mesh with new vertices
            watersMeshes[index].vertices = vertices;
            watersMeshes[index].SetTriangles(triangles, 0);
            watersMeshes[index].RecalculateBounds();
            watersMeshes[index].RecalculateNormals();
            watersMeshes[index].vertices = watersMeshes[index].vertices;
        }
        
        void Update()
        {
            if (Time.frameCount % interval == frameIntervalToWorkOn)
            {
                GenerateWaves();
            }
            else if (Time.frameCount % interval == frameIntervalToWorkOn+1)
            {
                GenerateOtherWaves();
            }
            else if (Time.frameCount % interval == frameIntervalToWorkOn+2)
            {
                
                GenerateLastWaves();
                
                
                for (int i = 0; i < watersMeshes.Length; i++)
                {
                    watersMeshes[i].RecalculateNormals();
                    watersMeshes[i].MarkDynamic();
                    watersMeshes[i].vertices = vertices;
                }
                currentTime = Time.time;
            }            
        }

        /// <summary>
        /// Based on the specified wave height and frequency, generate 
        /// wave motion originating from waveOriginPosition
        /// </summary>
        void GenerateWaves()
        {
            
            for (int i = 0; i < (vertices.Length/5)*2; i++)
            {
                Vector3 v = vertices[i];

                //Initially set the wave height to 0
                v.y = 0.0f;

                //Get the distance between wave origin position and the current vertex
                float distance = Vector3.Distance(v, waveOriginPosition);
                distance = (distance % waveLength) / waveLength;

                //Oscilate the wave height via sine to create a wave effect
                v.y = waveHeight * Mathf.Sin(currentTime * Mathf.PI * 2.0f * waveFrequency
                + (Mathf.PI * 2.0f * distance));
                
                //Update the vertex

                vertices[i] = v;
            }
        }
        void GenerateOtherWaves()
        {
            for (int i = (vertices.Length/5)*2; i < (vertices.Length/5)*4; i++)
            {
                Vector3 v = vertices[i];

                //Initially set the wave height to 0
                v.y = 0.0f;

                //Get the distance between wave origin position and the current vertex
                float distance = Vector3.Distance(v, waveOriginPosition);
                distance = (distance % waveLength) / waveLength;

                //Oscilate the wave height via sine to create a wave effect
                v.y = waveHeight * Mathf.Sin(currentTime * Mathf.PI * 2.0f * waveFrequency
                                             + (Mathf.PI * 2.0f * distance));
                
                //Update the vertex

                vertices[i] = v;

            }
        }
        void GenerateLastWaves()
        {
            for (int i = (vertices.Length/5)*4; i < vertices.Length; i++)
            {
                Vector3 v = vertices[i];

                //Initially set the wave height to 0
                v.y = 0.0f;

                //Get the distance between wave origin position and the current vertex
                float distance = Vector3.Distance(v, waveOriginPosition);
                distance = (distance % waveLength) / waveLength;

                //Oscilate the wave height via sine to create a wave effect
                v.y = waveHeight * Mathf.Sin(currentTime * Mathf.PI * 2.0f * waveFrequency
                                             + (Mathf.PI * 2.0f * distance));
                
                //Update the vertex

                vertices[i] = v;

            }
        }
        
    }
    
