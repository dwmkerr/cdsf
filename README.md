# cdsf

[![GuardRails badge](https://badges.production.guardrails.io/dwmkerr/cdsf.svg)](https://www.guardrails.io)

Project Description
The Composite Data Service Framework is a toolkit that extends the functionality of the WCF Data Services APIs by allowing a set of OData Services from distinct data sources to be aggregated into a single Data Service, with client-side APIs to help with common tasks.

What is the Composite Data Service Framework for?

WCF Data Services provide a fantastic way to rapidly build a service which exposes data from Entity Framework or CLI classes via a REST/OData service. However, there are some limitations:

It is impossible to aggregate many data sources into a single OData service.
It is impossible to augment the Data Service with traditional WCF service operations.
OData Service Operations are supported, but no client-side proxies are generated for them.
Service Operations are limited to 2KB of message data, which may be unsuitable for certain scenarios. 
The Composite Data Service Framework resolves these problems by allowing many data sources to be aggregated and presented as a single OData service, with the possibility to leverage traditional WCF service operations in the same service. As well as this, there is greater support for Service Operations and generation of Service Operation proxies client side.

A further benefit of the Composite Data Service Framework is the capability to extend the functionality of Services using components written with the Managed Extensibility Framework - allowing a plugin based approach to adding extra business logic and capabilities.

Development Status

The first release of the CDSF is under development at the moment. You can keep up-to-date by following the development on my blog:

www.dwmkerr.com

I am looking for suggestions as well, please get involved via the Discussion page.

Targets

The Targets of this project are outlined below, each release will address one target.

Target 1: Allow multiple WCF Data Services to be Aggregated into a single Composite Data Service

The primary function of this project is to provide composition of data services.

Target 2: Provide Client-Side support of [ProxyType]

The code generated client-side should allow the [ProxyType] attribute to mark a property as client-side only.

Target 3: Provide Client-Side generation of Service Operation Proxies

The code generated client-side should generate proxies for service operations.

Target 4: Address Limitations of Service Operations

The framework should support POST Service Operations.

Target 5: Provide Streaming Support

It should be possible to mark a resource as a streaming resource, with a MIME type, with a streaming service provider supplying access to the resource.
