

using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace Dkeshri.SystemDesign.LowLevel.Basic
{

    public class VideoEncoder
    {
        //1. define Delegate
        //2. define Event based on delegate
        //3. raise the event.
        public string EncoderName = "Deepak video Encoder";
        public delegate void VideoEncodedEventHandler(object sender, EventArgs eventArgs); // define delegate
        public event VideoEncodedEventHandler VideoEncoded; // defaine event.

        // Action delegate defined in system namespace.
        //the Action delegate doesn't return a value.
        // In other words, an Action delegate can be used with a method that has a void return type.
        public event Action<int,string> EventwithActionDelegate;



        public void Encode()
        {

            Console.WriteLine("Video Encoding.....");
            Thread.Sleep(3000);

            onVideoEncoded(); // raise Event
        }
        protected virtual void onVideoEncoded()
        {
            if (VideoEncoded != null)
                VideoEncoded(this, EventArgs.Empty);
            if (EventwithActionDelegate != null)
            {
                EventwithActionDelegate(5,"Deepak");
            }
        }
    }
    public class EventAndDelegateEx : IExecute
    {
        public void run()
        {
            VideoEncoder videoEncoder = new VideoEncoder(); // Publisher who fire event.
            Mailservice mailservice = new Mailservice(); // subscriber1
            MessageService messageService = new MessageService(); // subscriber 2
            videoEncoder.VideoEncoded += mailservice.onVideoEncoded;
            videoEncoder.VideoEncoded += messageService.sentMessage;
            videoEncoder.EventwithActionDelegate += mailservice.EventwithActionDelegateHandler;


            videoEncoder.Encode();


            // we can Also remove the subscriber already subscribed to the event the event too...
            // by using -= operator.
            videoEncoder.VideoEncoded -= mailservice.onVideoEncoded;
            Thread.Sleep(3000);
            videoEncoder.Encode();





        }
    }
    public class Mailservice
    {
        public void onVideoEncoded(object sender, EventArgs args)
        {
            VideoEncoder v = (VideoEncoder)sender;
            Console.WriteLine(v.EncoderName);
            Console.WriteLine("Sending mail");
        }
        public void EventwithActionDelegateHandler(int a,string s)
        {
            Console.WriteLine(a+" "+ s);
        }
    }
    public class MessageService
    {
        public void sentMessage(object sender, EventArgs args)
        {
            Console.WriteLine("Sending Message");
        }
    }
}