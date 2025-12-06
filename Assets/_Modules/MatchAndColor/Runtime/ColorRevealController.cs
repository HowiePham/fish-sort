using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorRevealController : MonoBehaviour
{
    [Header("References")]
    public Material revealMaterial; // Material sử dụng ColorRevealMultiple shader
    
    [Header("Reveal Settings")]
    public float revealSpeed = 3f; // Tốc độ màu loang ra
    public float maxRevealRadius = 5f; // Bán kính tối đa mỗi vòng tròn
    public int maxReveals = 20; // Số lượng vòng tròn tối đa
    
    [Header("Object Detection")]
    public LayerMask objectLayer; // Layer của các đồ vật cần tìm
    
    [Header("Visual Effects")]
    public bool showDebugCircles = true; // Hiển thị vòng tròn debug
    public Color debugCircleColor = Color.yellow;
    
    private List<RevealCircle> activeReveals = new List<RevealCircle>();
    private Vector4[] revealPositionsArray; // Array để gửi lên shader
    
    // Class để lưu thông tin mỗi vòng tròn reveal
    private class RevealCircle
    {
        public Vector3 position;
        public float currentRadius;
        public float targetRadius;
        public bool isExpanding;
        public float lifetime; // Thời gian tồn tại
        
        public RevealCircle(Vector3 pos, float maxRadius)
        {
            position = pos;
            currentRadius = 0f;
            targetRadius = maxRadius;
            isExpanding = true;
            lifetime = 0f;
        }
    }
    
    void Start()
    {
        if (revealMaterial == null)
        {
            Debug.LogError("Please assign a material with ColorRevealMultiple shader!");
            return;
        }
        
        // Khởi tạo array
        revealPositionsArray = new Vector4[maxReveals];
        
        // Khởi tạo shader với 0 reveals
        revealMaterial.SetInt("_RevealCount", 0);
    }
    
    void Update()
    {
        // Xử lý click chuột hoặc touch
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPos.z = 0;
            
            // Kiểm tra có click vào object không
            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero, 0f, objectLayer);
            
            if (hit.collider != null)
            {
                Debug.Log("Found object: " + hit.collider.name);
                
                RevealColorAt(clickPos);
                
                // Gọi event trên object nếu có
                FindableObject findable = hit.collider.GetComponent<FindableObject>();
                if (findable != null)
                {
                    findable.OnFound();
                }
            }
        }
        
        // Update tất cả các reveal circles
        UpdateRevealCircles();
    }
    
    public void RevealColorAt(Vector3 position)
    {
        RevealColorAt(position, maxRevealRadius);
    }
    
    public void RevealColorAt(Vector3 position, float customRadius)
    {
        // Nếu đã đạt max reveals, xóa cái cũ nhất
        if (activeReveals.Count >= maxReveals)
        {
            activeReveals.RemoveAt(0);
        }
        
        // Tạo một reveal circle mới
        RevealCircle newReveal = new RevealCircle(position, customRadius);
        activeReveals.Add(newReveal);
    }
    
    void UpdateRevealCircles()
    {
        // Update tất cả các reveal circles
        for (int i = activeReveals.Count - 1; i >= 0; i--)
        {
            RevealCircle reveal = activeReveals[i];
            reveal.lifetime += Time.deltaTime;
            
            if (reveal.isExpanding)
            {
                // Mở rộng radius
                reveal.currentRadius += revealSpeed * Time.deltaTime;
                
                if (reveal.currentRadius >= reveal.targetRadius)
                {
                    reveal.currentRadius = reveal.targetRadius;
                    reveal.isExpanding = false;
                }
            }
            
            // Cập nhật vào array để gửi lên shader
            if (i < maxReveals)
            {
                revealPositionsArray[i] = new Vector4(
                    reveal.position.x,
                    reveal.position.y,
                    reveal.position.z,
                    reveal.currentRadius
                );
            }
        }
        
        // Gửi dữ liệu lên shader
        if (revealMaterial != null && activeReveals.Count > 0)
        {
            revealMaterial.SetInt("_RevealCount", activeReveals.Count);
            revealMaterial.SetVectorArray("_RevealPositions", revealPositionsArray);
        }
        else if (revealMaterial != null)
        {
            revealMaterial.SetInt("_RevealCount", 0);
        }
    }
    
    // Vẽ debug circles trong Scene view
    void OnDrawGizmos()
    {
        if (!showDebugCircles || activeReveals == null) return;
        
        Gizmos.color = debugCircleColor;
        foreach (var reveal in activeReveals)
        {
            DrawCircle(reveal.position, reveal.currentRadius);
        }
    }
    
    void DrawCircle(Vector3 center, float radius)
    {
        int segments = 36;
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);
        
        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0
            );
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
    
    // Reset về trạng thái grayscale
    public void ResetReveal()
    {
        activeReveals.Clear();
        if (revealMaterial != null)
        {
            revealMaterial.SetInt("_RevealCount", 0);
        }
    }
    
    // Reveal tất cả màn hình
    public void RevealAll()
    {
        StartCoroutine(RevealAllCoroutine());
    }
    
    IEnumerator RevealAllCoroutine()
    {
        // Tạo grid các điểm reveal để phủ toàn màn hình
        Camera cam = Camera.main;
        float height = cam.orthographicSize * 2;
        float width = height * cam.aspect;
        
        int rows = 5;
        int cols = 5;
        
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Vector3 pos = new Vector3(
                    -width/2 + (width / (cols-1)) * x,
                    -height/2 + (height / (rows-1)) * y,
                    0
                );
                
                RevealColorAt(pos, maxRevealRadius * 1.5f);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    
    // Lấy số lượng objects đã tìm được
    public int GetFoundObjectsCount()
    {
        return activeReveals.Count;
    }
}

