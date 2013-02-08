#PostageApp .NET

This client library will allow you to quickly send email from a .NET 3.5+ application via the [PostageApp](http://postageapp.com) API. 
Specify one or more recipients, your template along with variables to substitute, enjoy open and click tracking, and quick, reliable delivery.

## Add library reference

### Via NuGet

To install PostageApp, run the following command in the *Package Manager Console*:

	PM> Install-Package PostageApp

### Downloading the compiled binary

The latest binary package can be found [here](http://postageapp.com/postageapp-net/PostageApp.0.0.1.zip).

## Obtaining an API key

Visit [postageapp.com/register](https://secure.postageapp.com/register) and sign-up for an account. Create one or more projects
in your account each project gets its own API key. Click through to the project page and find the API key in the right-hand column.

## Sending an email

The following is a the absolute minimum required to send an email.

	var client = new PostageApp.Client("YOUR_API_KEY");

	client.SendMessage(new SendMessageRequest()
		{
			Recipient = "Alan Smithee <alan.smithee@gmail.com>",
			RecipientOverride = "YOUR_EMAIL_ADDRESS_HERE_DURING_DEVELOPMENT",
			Subject = "Thank you for your order",
			From = "Acme Widgets <widgets@acme.com>",
			Text = "Your order has been processed and will ship shortly.",
			Html = "<p>Your order has been processed and will ship shortly.</p>"
		});

Setting the `RecipientOverride` property allows you to safely redirect all outgoing email to your own address while in development mode.

*Note - in these examples I am using the new object property initialization features of C# for berevity. 
The above can just as easily be written out in long-form, as in the example below.*

## Passing variables to templates

The real power of PostageApp kicks in when you start using templates. Templates can be configured in your PostageApp project dashboard. 
They can inherit from other templates, contain both text and html representations, provide placeholders for variables, headers and more. 
Your app doesn't need to concern itself with rendering html emails and you can update your email content without re-deploying your app. 

Once you have created a template that you want to use, specify its unique `slug` in the Template property as in the example below.

	client.SendMessage(new SendMessageRequest()
		{
			Recipient = "Alan Smithee <alan.smithee@gmail.com>",
			Template = "YOUR_TEMPLATE_SLUG",
			Variables = new Dictionary<string, string>()
				{
					{"first_name", "Alan"},
					{"last_name", "Smithee"},
					{"order_id", "555"}
			   }
		});

## Multiple recipients

Emails aren't restricted to just one recipient. Instead of setting the `Recipient` property, set the `Recipients` property
to a list of `Recipient` objects, each with its own set of variables.

    var sendMessageRequest = new SendMessageRequest()
        {
            Template = "YOUR_TEMPLATE_SLUG"
        };

    sendMessageRequest.Recipients.Add(new Recipient("Alan Smithee <alan.smithee@gmail.com>")
        {
            Variables = new Dictionary<string, string>()
                {
                    {"first_name", "Alan"},
					{"last_name", "Smithee"},
			        {"order_id", "555"}
                }
        });

    sendMessageRequest.Recipients.Add(new Recipient("Rick James <rick.james@gmail.com>")
        {
            Variables = new Dictionary<string, string>()
                {
                    {"first_name", "Rick"},
                    {"last_name", "James"},
                    {"order_id", "556"}
                }
        });       

## Attaching files

In addition to attaching files to templates in the PostageApp project dashboard, they can be attached by your app at runtime.
Simply add an `Attachment` to the `Attachments` collection, providing a `Stream`, Filename and ContentType for each file attached.

    sendMessageRequest.Attachments.Add(new Attachment(fileStream, "invoice.pdf", "application/pdf"));

## Adding custom headers

The `From`, `Subject` and `ReplyTo` properties are shortcuts for the following syntax.

	sendMessageRequest.Headers.Add("From", "Acme Widgets <widgets@acme.com>");
	sendMessageRequest.Headers.Add("Subject", "Your order has shipped!");
	sendMessageRequest.Headers.Add("Reply-To", "Acme Support <support@acme.com>");

You are free to add any necessary email headers using this method.

## Handling exceptions

An attempt is made to catch all internal `WebException`s and re-throw them as `SendMessageException`s, with the servers
error message and status code parsed from the response.

Common exceptions:

**400** - *Bad Request*: You have not provided enough information to send an email.  
**401** - *Unauthorized*: Invalid API key.
**412** - *Precondition Failed*: Some part of the request is invalid. Incorrect template slug? Invalid email address?

## Building the Nuget package

This library is published as a NuGet package, which can be rebuilt after modifications have been made by issuing the following commands.

	NuGet.exe pack PostageApp\PostageApp.csproj

You can test locally by specifying your project folder as a NuGet repository source and adding the package to a new project.

When you are ready to publish, don't forget to bump the AssemblyInfo version number for the project, build and then run the following commands to push.

	NuGet.exe setApiKey YOUR_API_KEY
	NuGet.exe push PostageApp.X.X.X.nupkg
