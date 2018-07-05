#PostageApp .NET

This client library will allow you to quickly send email from a .NET Core application via the [PostageApp](http://postageapp.com) API.
Specify one or more recipients, your template along with variables to substitute, enjoy open and click tracking, and quick, reliable delivery.

## Add library reference

### Via NuGet

To install PostageApp, run the following command in the *Package Manager Console*:

```
    PM> Install-Package PostageApp
```

## Obtaining an API key

Visit [postageapp.com/register](https://secure.postageapp.com/register) and sign-up for an account. Create one or more projects
in your account each project gets its own API key. Click through to the project page and find the API key in the right-hand column.

## Sending an email

In general you will use .net core DI infrastructure.

Registration:

```cs

services.AddPostageAppClient(options => {
    options.ApiKey = "Your Key";
});

```

Using:

```cs

class MyController
{
    private readonly IPostageAppClient _postageAppClient;

    MyController(IPostageAppClient postageAppClient)
    {
        _postageAppClient = postageAppClient;
    }


    public async Task<ActionResult> Action()
    {
        GetAccountInfoResult result = await _postageAppClient.GetAccountInfoAsync();
        if (!result.Succeeded)
        {
            // TODO error
        }

        // TODO action!

        return Ok();
    }
}

```

## Passing variables to templates

The real power of PostageApp kicks in when you start using templates. Templates can be configured in your PostageApp project dashboard. 
They can inherit from other templates, contain both text and html representations, provide placeholders for variables, headers and more. 
Your app doesn't need to concern itself with rendering html emails and you can update your email content without re-deploying your app. 

Once you have created a template that you want to use, specify its unique `slug` in the Template property as in the example below.

```cs
    await client.SendMessageAsync(new Message
        {
            Recipients = new MessageRecipient("Alan Smithee <alan.smithee@gmail.com>"),
            Template = "YOUR_TEMPLATE_SLUG",
            Variables = new Dictionary<string, string>()
                {
                    {"first_name", "Alan"},
                    {"last_name", "Smithee"},
                    {"order_id", "555"}
               }
        });
```

## Multiple recipients

Emails aren't restricted to just one recipient. Set the `Recipients` property
to a list of `MessageRecipient` objects, each with its own set of variables.

```cs
    var message = new Message();

    message.Recipients = new []
    {
        new MessageRecipient("Alan Smithee <alan.smithee@gmail.com>", new Dictionary<string, string>()
                {
                    {"first_name", "Alan"},
                    {"last_name", "Smithee"},
                    {"order_id", "555"}
                }),

        new MessageRecipient("Rick James <rick.james@gmail.com>", new Dictionary<string, string>()
                {
                    {"first_name", "Rick"},
                    {"last_name", "James"},
                    {"order_id", "556"}
                }),
    }
```

## Attaching files

In addition to attaching files to templates in the PostageApp project dashboard, they can be attached by your app at runtime.
Simply add an `MessageAttachment` to the `Attachments` collection, providing a File bytes, Filename and ContentType for each file attached.

```cs
    message.Attachments.Add("invoice.pdf", new MessageAttachment(fileBytes, "application/pdf"));
```

## Adding custom headers

The `From`, `Subject` and `ReplyTo` properties are shortcuts for the following syntax.

    message.Headers.Add("from", "Acme Widgets <widgets@acme.com>");
    message.Headers.Add("subject", "Your order has shipped!");
    message.Headers.Add("reply-to", "Acme Support <support@acme.com>");

You are free to add any necessary email headers using this method.
