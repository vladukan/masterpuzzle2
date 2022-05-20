using UnityEngine;
using Unity.Notifications.Android;
public class NotificationsManager : MonoBehaviour
{
    public int days = 14;
    private void Awake()
    {
        CreateChannel();
    }
    private void Start()
    {
        AndroidNotificationCenter.CancelAllNotifications();
        for(int i = 1; i<=days;i++) CreateNotification("present", i);
    }
    private void CreateChannel()
    {
        AndroidNotificationChannel channel = new AndroidNotificationChannel()
        {
            Name = "Presents",
            Description = "Notifications about new present",
            Id = "present",
            Importance = Importance.Low
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
    public void CreateNotification(string channel, int delay)
    {
        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Daily gift",
            Text = "Claim your daily gift",
            FireTime = System.DateTime.Now.AddDays(delay),
            SmallIcon = "icon_small",
            LargeIcon = "icon_large"
        };
        AndroidNotificationCenter.SendNotification(notification, channel);
    }
}
