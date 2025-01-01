using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold_Obj : MonoBehaviour
{
    public eShop_Value shop_Value;
    private bool bFollw;
    private Player player;

    private const float fSpeed = 13f;

    public void Init()
    {
        if (player == null)
            player = ModelManager.Instance.player;
        bFollw = false;
    }
    public void Update_Gold()
    {
        if (!bFollw)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < player.fFull_Item)
            {
                bFollw = true;
            }
        }

        if (bFollw)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * fSpeed);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            float _gold = Random.Range(5, 11) * GameManager.Instance.localGame_DB.Get_Stage();
            switch (shop_Value)
            {
                case eShop_Value.Gold:
                    GameManager.Instance.localGame_DB.Set_PlusGold((int)(_gold + (_gold * player.fGold_Increment)));
                    break;
                case eShop_Value.Crystal:
                    GameManager.Instance.localGame_DB.Set_PlusCrystal((int)_gold);
                    break;
            }
            UIManager.Instance.Set_Gold();

            ModelManager.Instance.Return_Gold(shop_Value, this);
        }
    }
}
