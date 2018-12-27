using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Side {Player, Opponent};
public enum Option {FaceUp, FaceDown};

public class DiscardArea : MonoBehaviour
{
    public Side side;
    public Option option;

    // Start is called before the first frame update
    void Start()
    {
        if (side == null || option == null) {
            throw new Exception("DiscardArea needs both side and option");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
