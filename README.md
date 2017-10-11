# Mobile application development labs
(UWP,C#) ---- MVVM ---- year 4 
## data binding
##### read from databases
App ==> [Text file](https://github.com/TangqiFeng/FWIW/blob/master/MVVM%20Lab.pdf)
##### read from json file
App9databind ==> [Text file](https://github.com/TangqiFeng/FWIW/blob/master/MVVM%20Lab%20part%202.pdf)

# Why MVVM?

Before we look at the code, I wanted to touch on whether it is worth the effort to use an approach like MVVM (Model, View, ViewModel).  The basics of MVVM are pretty simple…

* Model.  Think of the models as the business objects.  A Model should know about nothing about the user experience - specifically the View or how it is implemented using a ViewModel.  The Model only interact with system and data services.  

* ViewModel.  The ViewModel is where you encapsulate any code or data that your UX or View will need.  It is important that ViewModel only knows about and encapsulates the Model – but it shouldn’t be responsible for any Business Logic or Business Constraints – that should all be in the model. And on the other side, the ViewModel should know nothing about the specifics of the Views and UX that use it.

* View.  This is where you create the user experience – in Windows apps you do this declaratively with XAML markup language and design tools.  The View uses properties and actions on the ViewModel to get the job done.

Since the MVVM approach is so conceptually simple, why do a lot of people get frustrated with it?  One challenge was that historically the approach was fairly opinionated requiring base classes like DependencyObject and implementations often required a lot of coding to do simple things like connecting up a command to XAML.  Another challenge was that a lot of developers felt that execution overhead of runtime bindings – and challenges debugging runtime bindings – ultimately meant they needed to abandon binding and simply do imperative code to implement the UX.

With Windows 10 and Visual Studio 2015 many of the challenges are have been addressed. As we show we can now easily build an MVVM app using compile-time bindings that eliminate runtime overhead and reduce the likelihood you will need to debug bindings at runtime. We also show how x:Bind to methods means we don’t need to implement Commands and we show how a new C# language feature [CallerMemberName] can help make it easier to implement INotificationPropertyChanged.

MVVM was already a well-proven approach that has been used to build many sophisticated and great performing apps and now with these new capabilities MVVM is more approachable than ever.

# An Overview of the App 

One of the key goals was to keep this example as simple as possible. But we also want the example to represent what a developer would actually need to do to build an app using MVVM. And getting all the CRUD operations in place over a real world database or other system isn’t trivial. So here is how we approach this.

At the core of the app is a Business Object – our Model.  It is called Organization.  It is just an in-memory “database” of People objects and an organization Name.  The People collection is just a list where each Person have a Name and Age. 

In terms of implementing the model we leverage some scaffolding code that implements a FakeDataService that pretends to communicate with a cloud service that maintains the data – but here our stubbed out service to run locally and just prints messages for debugging / logging purposes.  Again, the only reason to do this is so we have a realistic sense of what would be involved with building this over real world data and business objects.

In terms of the user experience all we need is a View that shows the list of people in the Organization and enables the user to Add or Delete People, or Update their Name and Age properties – nothing complicated.  Our View is written entirely in XAML with no code behind other than initializing the ViewModel.

Most of code in the sample is focused on building the ViewModel for the Organization business object. The OrganizationViewModel keeps track of a collection of People that is kept in sync with the model. It also keeps track of a SelectedPersonIndex from the collection of people so the View can perform edit and delete operations on the current selection.
