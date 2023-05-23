using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coach : MonoBehaviour
{
    public string Name { get; set; } // The name of the Coach
    public int level { get; set; }
    public List<Enums.EStrategy> Strategies { get; set; }

    /*

      // Start is called before the first frame update
      void Start()
      {

      }

      // Update is called once per frame
      void Update()
      {

      }
     */
}
