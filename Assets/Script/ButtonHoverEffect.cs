using UnityEngine;
using TMPro; // Thư viện cho chữ
using UnityEngine.EventSystems; // Thư viện để bắt sự kiện chuột

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI textMesh;

    [Header("Kéo 2 Material vào đây")]
    public Material normalMaterial; // Material thường
    public Material glowMaterial;   // Material phát sáng

    void Start()
    {
        // Tự động tìm component Text trong nút này
        textMesh = GetComponentInChildren<TextMeshProUGUI>();

        // Đảm bảo lúc đầu là material thường
        if (textMesh != null && normalMaterial != null)
        {
            textMesh.fontSharedMaterial = normalMaterial;
        }
    }

    // Khi chuột đưa vào
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textMesh != null && glowMaterial != null)
        {
            textMesh.fontSharedMaterial = glowMaterial; // Đổi sang áo phát sáng

            // Mẹo nhỏ: Tăng kích thước chữ lên xíu cho nảy
            transform.localScale = Vector3.one * 1.1f;
        }
    }

    // Khi chuột đưa ra
    public void OnPointerExit(PointerEventData eventData)
    {
        if (textMesh != null && normalMaterial != null)
        {
            textMesh.fontSharedMaterial = normalMaterial; // Về lại áo thường

            // Trả về kích thước cũ
            transform.localScale = Vector3.one;
        }
    }
}