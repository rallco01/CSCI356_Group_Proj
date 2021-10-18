//Generate Path
using UnityEngine;
using UnityEngine.AI;

public class ShowGoldenPath : MonoBehaviour
{
	public Vector3 destination;

	public NavMeshAgent agent;
	private NavMeshPath path;

	void start() {
	}

<<<<<<< HEAD
	public void updateDestination(Vector3 dest) {
		destination = dest;
	}
=======
    void drawPath() {
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
    }
>>>>>>> 70a26328768b3fa4f9f6c53346fa16b7c365dfd8

	void Update() {
		agent.SetDestination(destination);
		path = agent.path;

<<<<<<< HEAD
		//show the path of the nav mesh agent
		for (int i = 0; i < path.corners.Length - 1; i++)
			Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
	}

	
=======
        //show the path of the nav mesh agent
        drawPath();
    }

    public void updateDestination(Vector3 dest) {
        destination = dest;
    }

    public NavMeshPath getPath() {
        return path;
    }
>>>>>>> 70a26328768b3fa4f9f6c53346fa16b7c365dfd8
}