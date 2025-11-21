using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb; 
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0; 
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject door;
    public GameObject door2;
    public GameObject Door3;
    public GameObject Door4;
    public GameObject Enemy;
    public GameObject Enemy2; 
    public GameObject Enemy3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent <Rigidbody>(); 
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
        Enemy2.SetActive(false);
        Enemy3.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); 
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

    void SetCountText(){
        countText.text = "Count: " + count.ToString();
        if (count >= 2){
            door.SetActive(false);
            Enemy.SetActive(false);
            Enemy2.SetActive(true);
            Enemy3.SetActive(true);
        }
        if (count >=8){
            door2.SetActive(false);
            Enemy2.SetActive(false);
            Enemy3.SetActive(false);
        }
        if (count >=10){
            Door3.SetActive(false);
        }
        if (count >=11){
            Door4.SetActive(false);
        }
        if(count >=12){
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }
    
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count +1;

            SetCountText();
        }
        
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Enemy"))
        {
       Destroy(gameObject); 
      
       winTextObject.gameObject.SetActive(true);
       winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
}
