using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullete : MonoBehaviourPun
{
    public float speed = 10f;
    public float destroyTime = 2f;
    public bool shootLeft = false;
    public SpriteRenderer bulletesr;

    IEnumerator destroyBullete()
    {
        yield return new WaitForSeconds(destroyTime);
        this.GetComponent<PhotonView>().RPC("OnDestroy", RpcTarget.AllBuffered);

    }

    void Start()
    {
        StartCoroutine(destroyBullete());
    }

    
    void Update()
    {
        if(!shootLeft)
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        else
            transform.Translate(Vector2.left * Time.deltaTime * speed);
    }
    [PunRPC]
    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }
    [PunRPC]
    public void changeDirection()
    {
        shootLeft = true;
        bulletesr.flipX = true;
    }

}
