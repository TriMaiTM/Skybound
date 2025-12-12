using System.Collections;
using UnityEngine;

public class WindController : MonoBehaviour
{
    [Header("Cấu hình Thời gian")]
    public float thoiGianThoi = 5f;   
    public float thoiGianNghi = 2f;  

    [Header("Cấu hình Vật lý & Hình ảnh")]
    public AreaEffector2D vungGioPhysics; // 
    public ParticleSystem vungGioVisual;  // 

    [Header("Vị trí Mốc")]
    public Transform posLeft;  // Kéo Pos_Left vào đây
    public Transform posRight; // Kéo Pos_Right vào đây

    private void Start()
    {
        
        StartCoroutine(ChuKyGio());
    }

    IEnumerator ChuKyGio()
    {
        while (true) // Lặp vô tận
        {
            
            Debug.Log("Gió thổi: Trái -> Phải");

            // 1. Di chuyển máy quạt về bên Trái
            vungGioVisual.transform.position = posLeft.position;
            // 2. Xoay máy quạt sang Phải (Y = 90)
            vungGioVisual.transform.rotation = Quaternion.Euler(0, 90, 0);

            // 3. Bật gió (Vật lý hướng 0 độ = sang Phải)
            BatGio(0f);

            yield return new WaitForSeconds(thoiGianThoi);

            // === PHA 2: NGHỈ ===
            Debug.Log("Gió ngừng");
            TatGio();
            yield return new WaitForSeconds(thoiGianNghi);

            // === PHA 3: THỔI TỪ PHẢI SANG TRÁI ===
            Debug.Log("Gió thổi: Phải -> Trái");

            // 1. Di chuyển máy quạt về bên Phải
            vungGioVisual.transform.position = posRight.position;
            // 2. Xoay máy quạt sang Trái (Y = -90 hoặc 270)
            vungGioVisual.transform.rotation = Quaternion.Euler(0, -90, 0);

            // 3. Bật gió (Vật lý hướng 180 độ = sang Trái)
            BatGio(180f);

            yield return new WaitForSeconds(thoiGianThoi);

            // === PHA 4: NGHỈ ===
            Debug.Log("Gió ngừng");
            TatGio();
            yield return new WaitForSeconds(thoiGianNghi);
        }
    }

    void BatGio(float huongLuc)
    {
        // 1. Xử lý Vật lý
        vungGioPhysics.enabled = true;
        vungGioPhysics.forceAngle = huongLuc;

        // 2. Xử lý Hình ảnh
        // Đảm bảo hệ thống đang chạy
        if (!vungGioVisual.isPlaying)
        {
            vungGioVisual.Play();
        }

        // Bật van xả hạt
        var emission = vungGioVisual.emission;
        emission.enabled = true;
    }

    void TatGio()
    {
        // Tắt Effect Vật lý
        vungGioPhysics.enabled = false;

        // Tắt phun hạt (hạt cũ sẽ bay nốt rồi hết, nhìn tự nhiên hơn)
        var emission = vungGioVisual.emission;
        emission.enabled = false;
    }
}