using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Obstacle component'lerinin değişkenleri
    private Rigidbody rigidbody;
    private MeshRenderer meshRenderer;
    private Collider collider;
    private ObstacleController obstacleController;

    private void Awake()
    {
        // Component'lerin tanımı
        rigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        obstacleController = transform.parent.GetComponent<ObstacleController>();
    }

    public void Shatter() // Objeleri shatter eden animasyon fonksiyonu
    {
        rigidbody.isKinematic = false; // Kinematic özelliğini kapatıldı(yani hareket özelliği verildi)
        collider.enabled = false; // Parçalanan nesneler çarpışmasın diye collider özelliğini kapatıldı

        Vector3 Point = transform.parent.position; // Parent modelin posizyon noktası
        float parentPOSx = transform.parent.position.x; // Parent modelin x pozisyon değeri
        float POSx = meshRenderer.bounds.center.x; // Mesh renderer'ın sınırlarının orta noktasının x değeri

        Vector3 yanYön = (parentPOSx - POSx < 0) ? Vector3.right : Vector3.left; // üçlü karşılastırma ile 0'dan kucukse sağa, büyükse sola atıyoruz (yan yön belirleme)

        Vector3 Yön = (Vector3.up *  1.5f + yanYön).normalized; // Vektöre bir güç veriyoruz ve yönünün değişmemesi için normalized yapıyoruz


        float guc = Random.Range(20, 35); // Random bir güç tanımlıyoruz
        float tork = Random.Range(110, 180); // Random bir tork tanımlıyoruz

        rigidbody.AddForceAtPosition(Yön * guc, Point, ForceMode.Impulse); // rigidbody'nin bulunduğu poziyonda, yönüne güç veriyoruz

        rigidbody.AddTorque(Vector3.left * tork); // rigidbody'ye sağ yönünde tork ekliyoruz

        rigidbody.velocity = Vector3.down; // rigidbody'ye yukarı yönüne hız ekliyoruz
    }

}
