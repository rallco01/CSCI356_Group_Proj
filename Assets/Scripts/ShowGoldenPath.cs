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

    void drawPath() {
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
    }

	void Update() {
		agent.SetDestination(destination);
		path = agent.path;

        //show the path of the nav mesh agent
        drawPath();
    }

    public void updateDestination(Vector3 dest) {
        destination = dest;
    }

    public NavMeshPath getPath() {
        return path;
    }
}