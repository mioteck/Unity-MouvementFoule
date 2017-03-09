using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BodyPart : MonoBehaviour {

    public enum Face
    {
        X,
        Y,
        Z
    }

    public void AttachBodyPart(Face face, Vector2 coord, float reflection, GameObject go)
    {
        CharacterJoint characterJoint = gameObject.AddComponent<CharacterJoint>();
        Vector3 anchor = new Vector3();
        Vector3 connectedAnchor = new Vector3();
        Vector3 size = transform.localScale;
        if (face == Face.X)
        {
            anchor.Set(reflection * size.x, coord.x, coord.y);
            connectedAnchor.Set(-reflection * size.x, coord.x, coord.y);
            go.transform.position = transform.position + new Vector3(reflection * size.x, coord.x, coord.y);
        }
        else if (face == Face.Y)
        {
            anchor.Set(coord.x, reflection * size.y, coord.y);
            connectedAnchor.Set(coord.x, -reflection * size.y, coord.y);
            go.transform.position = transform.position + new Vector3(coord.x, reflection * size.y, coord.y);
        }
        else if (face == Face.Z)
        {
            anchor.Set(coord.x, coord.y, reflection * size.z);
            connectedAnchor.Set(coord.x, coord.y, -reflection * size.z);
            go.transform.position = transform.position + new Vector3(coord.x, coord.y, reflection * size.z);
        }

        characterJoint.anchor = anchor;
        characterJoint.connectedAnchor = connectedAnchor;
        characterJoint.connectedBody = go.GetComponent<Rigidbody>();
        characterJoint.enableCollision = false;
        characterJoint.autoConfigureConnectedAnchor = false;
    }

}
