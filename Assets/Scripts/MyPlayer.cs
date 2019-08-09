using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UI;

public class MyPlayer : MonoBehaviourPun,IPunObservable
{

    public PhotonView photonview;

    public float moveSpeed = 10;
    public float jumpforce = 800;

    private Vector3 smoothMove;

    private GameObject sceneCamera;
    public GameObject playerCamera;
    public SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool IsGround;
    public Text nameText;
    public GameObject bulletePreab;
    public Transform bulleteSpawn;
    public Transform bulleteSpawnLeft;

    void Start()
    {
        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate =15;
        if (photonView.IsMine)
        {
            nameText.text = PhotonNetwork.NickName;
            rb = GetComponent<Rigidbody2D>();
            sceneCamera = GameObject.Find("Main Camera");
            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
        }
        else
        {
            nameText.text = photonView.Owner.NickName;
        }
    }
        void Update()
    {

       if (photonview.IsMine)
       {
            ProcessInputs();           
       }
       else
        {
            smoothMovement();
        }

    }

    private void smoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 10);
    }

    private void ProcessInputs()
    {
        var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += move * moveSpeed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sr.flipX = false;
            photonView.RPC("OnDriectionChange_RIGHT", RpcTarget.Others);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sr.flipX = true;
            photonView.RPC("OnDriectionChange_LEFT", RpcTarget.Others);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Shoot();
        }
    }

    public void Shoot()
   
    {
        GameObject bullete;

        if (sr.flipX == true)
        {
            bullete = PhotonNetwork.Instantiate(bulletePreab.name, bulleteSpawnLeft.position, Quaternion.identity);
            bullete.GetComponent<PhotonView>().RPC("changeDirection", RpcTarget.AllBuffered);
        }

        if (sr.flipX == false)
        {
            bullete = PhotonNetwork.Instantiate(bulletePreab.name, bulleteSpawn.position, Quaternion.identity);
            
        }
    
    }



    [PunRPC]
    void OnDriectionChange_LEFT()
    {
        sr.flipX = true; 
    }
    [PunRPC]
    void OnDriectionChange_RIGHT()
    {
        sr.flipX = false;
    }


     void OnCollisionEnter2D(Collision2D collision)
    {
        if (photonview.IsMine)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                IsGround = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (photonview.IsMine)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                IsGround = false;
            }
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up *jumpforce);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            smoothMove = (Vector3)stream.ReceiveNext();
        }
    }
}
