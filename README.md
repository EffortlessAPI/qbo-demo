# Effortless API Project CLI

This seed will create a fully functional, secure implementation with access to your
EffortlessAPI endpoint for easy automation on the scripting platform of your choice.

## Installation with the EffortlessAPI CLI `npm install -g eejai42/eapi-cli`
> eapi -cloneEAPISeed project-cli ProjectXYCli your-project-alias

## Manual Installation
Creating a CLI for your project involves the following steps:
1. Clone this repository to a folder called `/your-cli` (assuming your EffortlessAPI Project is called `bob-your-project` for example.)
2. Update `/SSoTmeProject.json` to replace `demo-sd1` with `bob-your-project`
3. Update `/DotNet/CLIClassLibrary/EAPICLIHandler.cs` to replace `demo-sd1` with `bob-your-project`.
4. Update `/package.json` to replace `demo-sd1` with `bob-your-project`.
5. Build the ssot.me code: `ssotme -build`
6. Install the NPM package locally to test `npm install . -g`
7. Test the package: `your-cli help`
   
See below for further help/instructions.

## Usage
The following behaviors are common across all CLI's.  The specifics beyond what
is described here will depend on the specific EffortlessAPI project being
managed.

### Authentication
Any CLI will include simple commands for authenticating with any of the methods supported

> your-cli -authenticate you@domain.com -demoPassword Password123
> your-cli -authenticate you@domain.com -sha256HashedPassword xyzq23k321zyx
> your-cli -authenticate -withGoogle
> your-cli -authenticate -withFacebook
> your-cli -authenticate -withTwitter

### Registration
If your Guest role has Create permission on your `User` object, then your
CLI will include a `-regisiter you@domain.com` command line option.

### -as Role
By default, the first commands issued will be assumed to come from `-as Guest`, however, Any command can be updated to also include `-as Employee`, or `-as AnyRole` which will override the default.

It will also "remember" that request, and any subsequent requests which do not
specify the `-as ExplicitRole` will be invoked using the previous role.

### -help or help
Any any time you can request help as follows:
> your-cli -help GetInv
> your-cli -help
> your-cli help
> your-cli help -as Employee
> your-cli help -as Guest

This will list any commands which the given user can access.

> your-cli help GetCustomers -as Admin

Because this matches a specific method that Admin can reference, the
help will provide a detailed description of the method/payload, response, etc.

```Help for Admin.

Available Actions Matching: getcustomers
 - GetCustomers

* * * * * * * * * * * * * * * * * * * * * * * * * * *
* *  Customer     *
* *     - A customer of company XYZ, mostly online sales.  Created
* *       from Quickbooks
* * * * * * * * * * * * * * * * * * * * * * * * * * *

CRUD      - CustomerId
CRUD      - Name
CRUD      - Notes
CRUD      - Attachments
CRUD      - Status
CRUD      - Invoices
CRUD      - EmailAddress
CRUD      - User
CRUD      - UserEmail
```

There will also be a detailed Html description of how to interact with the
CLI as well at `/cli.html`.

