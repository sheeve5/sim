using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class Tile
    {
        private GameObject tileObject;
        private int positionX;
        private int positionY;
        private float temperature;
        private float humidity;
        private bool onFire;

        private float perlinTargetTemperature;
        private float perlinTargetHumidity;
        private float reactiveTemperature;
        private float reactiveHumidity;
        private int[] fireMessages = new int[8];
        private float delta = 0;
        private List<Tile> neighbors;

        public float Temperature { get => temperature; set => temperature = value; }
        public float Humidity { get => humidity; set => humidity = value; }
        public bool OnFire { get => onFire; set => onFire = value; }
        

        public void addNeighbor(Tile t) { neighbors.Add(t);  }

        public Tile(int x, int y, GameObject tile)
        {
            positionX = x;
            positionY = y;
            tileObject = GameObject.Instantiate(tile);
            tileObject.transform.position = new Vector3(x, y, 0);
            neighbors = new List<Tile>();
        }

        public void Update()
        {
            var temp = temperature - perlinTargetTemperature; //heat gained via reaction
            updatePerlin();
            
            temperature = perlinTargetTemperature;
            if (reactiveTemperature != 0)
            {
                temp += (float)(reactiveTemperature/1000);
            }
            else
            {
                temp -= 0.001f; //no fire nearby, lose accumulated heat
            }
            if (temp > 0)
            {
                temperature += temp;
            }
            temperature = Mathf.Clamp01(temperature);
            humidity = perlinTargetHumidity;
            if (UnityEngine.Random.value < (temperature * (1 - humidity))-0.4 && UnityEngine.Random.value < 0.01)
            {
                onFire = true;
            }
            if (onFire)
            {
                temperature = 1;
            }
            updateColor();
            delta += GlobalSettings.speed;
            reactiveTemperature = 0;

        }

        public void fireTick()
        {
            if(OnFire || temperature>perlinTargetTemperature)
                foreach(Tile t in neighbors)
                {
                    if (!t.onFire)
                    {
                        if (OnFire)
                            t.transferHeat(1);
                        else
                            t.transferHeat(temperature - perlinTargetTemperature);
                    }
                }
            
        }
        public void transferHeat(float heat)
        {
            reactiveTemperature += heat/8;
        }

        internal void ClearFire()
        {
            onFire = false;
            temperature = perlinTargetTemperature;
        }

        public void updatePerlin()
        {
            perlinTargetTemperature = Mathf.Clamp01(PerlinNoise3D((float)positionX/5, (float)positionY/5, delta));
            perlinTargetHumidity = Mathf.Clamp01(PerlinNoise3D((float)positionX/10, (float)positionY/10+ 1000, delta));
        }
        public void updateColor(){
            if (onFire)
            {
                tileObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                tileObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(temperature / 4 + 0.7f, (humidity/2 +0.5f), (1-(humidity/2)));    
            }
        }


        public static float PerlinNoise3D(float x, float y, float z)
        {
            float xy = Mathf.PerlinNoise(x, y);
            float xz = Mathf.PerlinNoise(x, z);
            float yz = Mathf.PerlinNoise(y, z);
            float yx = Mathf.PerlinNoise(y, x);
            float zx = Mathf.PerlinNoise(z, x);
            float zy = Mathf.PerlinNoise(z, y);

            return (xy + xz + yz + yx + zx + zy) / 6;
        }

        
    }

}
