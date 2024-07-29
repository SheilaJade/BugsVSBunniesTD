using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class TowerBehavior : MonoBehaviour
{

    SphereCollider myCollider;
    public float AttackRadius;
    //per second
    public float fireRate;
    public GameObject BulletPrefab;
    public GameObject BulletSource;
    // Start is called before the first frame update
    public List<GameObject> targetList = new List<GameObject>(); //We can switch GameObject to instances of the Enemy class
    void Start()
    {

        myCollider=GetComponent<SphereCollider>();
        myCollider.radius=AttackRadius;
        StartCoroutine(FireLoop());
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other+" "+other.GetInstanceID()+" entered");
        targetList.Add(other.gameObject);
        //printList(list);

    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other + " " + other.GetInstanceID()+ " exited");
        targetList.Remove(other.gameObject);
        //printList(list);

    }
    //for testing
    void printList(List<int> num)
    {
        string numbs = "{ ";
        for (int i = 0; i < num.Count; i++)
        {
            numbs = numbs+ (num[i].ToString()+", ");
        }
        numbs += " }";
        Debug.Log(numbs);

    }

    private IEnumerator FireLoop()
    {
        
        while (true)
        {
            if (targetList.Count > 0)
            {

                //pick a random enemy to attack
                int enemyID= Random.Range(0, targetList.Count - 1);
             //   Debug.Log(targetList[enemyID]);
                //Make sure the enemy is not already dead (is null)
                if (targetList[enemyID] != null)
                {
                    GameObject unit = targetList[enemyID];
                   // Debug.Log("PEW! hit enemy " + unit + " at location " + unit.transform.position);
                    BulletPrefab.GetComponent<bulletBehavior>().enemy = unit;
                    //prefab.GetComponent<bulletBehavior>().setTargetPosition(unit);
                    //the weird y position is so the bullet shoots from the top instaed of the middle. adjust as needed


                    //LookAt Code From Internet
                    if (unit != null)
                    {
                        // Get the direction to the enemy
                        Vector3 direction = unit.transform.position - transform.position;
                        direction.y = 0; // Keep the direction strictly on the Y-axis

                        // Create the rotation towards the enemy
                        Quaternion rotation = Quaternion.LookRotation(direction);

                        // Apply the rotation to the tower
                        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y+90, 0);
                    }


                    Vector3 spawnPos = transform.position + new Vector3(0, 1, 0);
                    if (BulletSource != null)
                    {
                        spawnPos = BulletSource.transform.position;
                    }
                    Instantiate(BulletPrefab, spawnPos, transform.rotation);

                    yield return new WaitForSeconds(fireRate);
                }
                else
                {//Lets remove the null instance and try again next loop interation.
                    targetList.RemoveAt(enemyID);
                }
            }

            yield return new WaitForEndOfFrame();
        }

        
    }
}
