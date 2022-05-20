using UnityEngine;
using System.Collections;
using Google.Play.Review;
public class ReviewController : MonoBehaviour
{
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;
    private void Start()
    {
        _reviewManager = new ReviewManager();
    }

    IEnumerator RequestReviewOperation()
    {
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        _playReviewInfo = requestFlowOperation.GetResult();
    }

    IEnumerator ReviewFlow()
    {
        if (_playReviewInfo != null)
        {
            var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
            yield return launchFlowOperation;
            _playReviewInfo = null; // Reset the object
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                yield break;
            }
        }
    }

    public void RequestReview()
    {
        print("RequestReview");
        if (Application.platform != RuntimePlatform.WindowsEditor)
            StartCoroutine(RequestReviewOperation());
    }

    public void ShowReview()
    {
                print("ShowReview");

        if (Application.platform != RuntimePlatform.WindowsEditor)
            StartCoroutine(ReviewFlow());
    }
}
