
using UnityEngine;
using System.Collections;
 
 // Thanks to `https://blog.csdn.net/ozhangseno/article/details/70799611`
 
public class Particle : MonoBehaviour
{
 
    public ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particlesArray;
    public int particleNumber = 2000;
    public float radius = 2.5f;
    public float[] particleAngle;
    public float[] particleRadius;
    public float maxR = 3f;
    public float speed = 0.05f;
    float time = 0;
    public float free = 0.05f;
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particlesArray = new ParticleSystem.Particle[particleNumber];//将储存粒子的数组初始化
        particleSystem.maxParticles = particleNumber;//设置粒子发射的最大数量
        particleAngle = new float[particleNumber];
        particleRadius = new float[particleNumber];
 
        particleSystem.Emit(particleNumber);//将刚刚初始化的particleNumber个粒子发射出去
        particleSystem.GetParticles(particlesArray);
        for (int i = 0; i < particleNumber; i++) {//为每个粒子设置其位置
            float angle = Random.Range(0.0f, 360.0f);//位置为0 - 360度的随机一个角度
            float rad = angle / 180 * Mathf.PI;//角度变换成弧度
            float midR = (maxR + radius) / 2;
            float rate1 = Random.Range(1.0f, midR / radius);
            float rate2 = Random.Range(midR / maxR, 1.0f);
            float r = Random.Range(radius * rate1, maxR * rate2);
 
            particleAngle[i] = angle;
            particleRadius[i] = r;
            particlesArray[i].position = new Vector3(r * Mathf.Cos(rad), r * Mathf.Sin(rad), 0.0f);//为每个粒子坐标赋值
        }
        particleSystem.SetParticles(particlesArray, particlesArray.Length);//设置该粒子系统的粒子。前面数组的长度是设置粒子的数量
    }
    void Update()
    {
        for (int i = 0; i < particleNumber; i++) {
            //设置速度为十个不同的档次
            if (i%2 == 0) {
                particleAngle[i] += speed*(i%10+1);
            } else {
                particleAngle[i] -= speed*(i%10+1);
            }
            if (particleAngle[i] > 360)
                particleAngle[i] -= 360;
            if (particleAngle[i] < 0)
                particleAngle[i] += 360;
            particleRadius[i] += (Mathf.PingPong(time, free) - free/2.0f);
            time += Time.deltaTime;
            time %= 100;
            float rad = particleAngle[i] / 180 * Mathf.PI;
            particlesArray[i].position = new Vector3(particleRadius[i] * Mathf.Cos(rad), particleRadius[i] * Mathf.Sin(rad), 0f);
        }
        particleSystem.SetParticles(particlesArray, particleNumber);
    }
 
}

