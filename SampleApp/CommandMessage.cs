namespace SampleApp;
public class CommandMessage : CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<Message>
{
    public CommandMessage(Message message) : base(message)
    {
    }
}

public enum MessageType
{
    SendSymbol,
    SendUndo,
    SendRedo,
    OpenOptions,
}

public class Message
{
    public MessageType Type { get; set; }
    public object Value { get; set; }
}
