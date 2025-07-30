# Repo_Rules_Processor_Proxy

This software component called "Repo_Rules_Processor_Proxy" and the concept is created in a collaborative project of members of the [IDTA](https://industrialdigitaltwin.org/) and [Open Industry 4.0 Alliance](https://openindustry4.com/) called "Exemplary industry testing of partial aspects of the proactive Asset Administration Shell". If you are interested in contributing or more details, please reach out.

In this project we are creating a concept how to describe the behaviour of assets in their Asset Administration Shell. Therefore we are creating rules which are stored/linked in the AAS of the Asset, when the rule is evaluated to true, an action is triggered, like sending publishing an event or writing a value to a defined SubmodelElement or call an API.

So we are able to bring the complex logic, which is nowadays (hard-)coded for example in MESs (Manufacturing Execution System) to a transparent and decentralized representation.

This proxy uses in the first step the approach with describing the rules with DMN (Decision Model and Notation, see [wikipedia](https://en.wikipedia.org/wiki/Decision_Model_and_Notation)).
Those DMN-files with the rules are stored in a File-SubmodelElement in a "Rules"- or "Behaviour"-Submodel in the AAS of the Asset. This submodel is not standardized, yet.

In the following sequence diagram the extraction of the DMN-files from this Submodel is encapsulated in the "RulesRepository".

### The concept in a nutshell:
- There is a **Proxy** for the AAS-/submodel repository. The API_Client is not aware of the proxy, because the proxy has exact the same API-Endpoints like the AAS-/submodel-repository.
- There are **Rules**. 
    - A Rule has at least one **SubmodelElementReferenceHook**, which is a reference to SubmodelElement in an AAS. Whenever this referenced SubmodelElement is touched (via a POST-, PUT-, DELETE- or maybe GET-request), the rule is relevant and must be evaluated.
    - A Rule can be a **PreRequestRule** or a **PostRequestRule**.
        - A PreRequestRule is evaluated before the proxy forwards the request from the API_Client to the repository. If one PreRequestRule is evaluated to false, the request is not forwared to the repository.
        - A PostRequestRule is evaluated after the proxy has forwarded the request to the repository and if the repository has HTTP response status code 2xx.
    - A Rule has a **DMN-File** which holds decision tables.  
        - *Structure*: A decision table is organized into columns and rows.
        - *Input Columns*: These represent the conditions (e.g., "Customer Age", "Order Amount"). Each input has a specific data type (like number, string, or boolean). In our context the conditions are described as paths to SubmodelElements. 
        - *Output Columns*: These represent the conclusions or results of the decision (e.g., "Discount Percentage", "Risk Level").
        - *Rules*: Each row in the table represents a single rule. A rule connects specific input values or ranges to specific output values. For a rule to be "true," all its input conditions must be met.
        - *Hit Policy*: A crucial part of the table is the "hit policy." It specifies what to do if multiple rules are true for a given set of inputs. Common policies include:
            - Unique (U): Only one rule can match.
            - First (F): The first matching rule, based on the row order, determines the output.
            - Collect (C): A list of all matching outputs is returned.
- There is a **RulesRepository**, which provides the rules for the requested SubmodelElement. The affected rules are selected by the SubmodelElementReferenceHook of the rule and the HTTP method. There can be many rules for one SubmodelElement. This RulesRepository is filled by the "Behaviour Submodels", which hold all rules.
- There is a **DMN_RulesParser**, which extracts the paths to SubmodelElements with the current value of the SubmodelElement.
- There is a **DMN_RulesEngine**, which evaluates the rule. This might be for example [Drools](https://drools.org/).



### Sequence Diagram

![Rules Engine Sequence Diagram](/diagrams/RulesEngine_Sequence.png)

The shown concept is not bound to DMN, you could also use another language to describe the rules. You have to ensure, the rule, the RulesParser, which extracts the paths to referenced SubmodelElement, and the RulesEngine, which evaluates the rule, are fitting together.

## Setup

### Setup Python Version of Proxy
1. Install Python 3. on your computer
2. Navigate to folder /Python in your terminal (on Windows you find the folder in PowerShell via 'Get-Command python', in the Bash via 'where python')
3. Create a Folder "secrets" with the following two files
    3.1. an empty file "__init__.py" to make this folder a Python package
    3.2. a file "url_details.py" containing the following three variables
        - base_url: the base url of your AAS repository
        - submodel_id: the base64-encoded version of the submodel_id that you want to query
        - header: the header information that has to be sent with each request to the AAS repository (including the ApiKey for the AAS designer)
3. Run python ProxyService.py

### Setup Drules/Knowledge is Everything Ecosystem
1. https://hub.docker.com/r/jboss/business-central-workbench-showcase
2. docker run -p 8080:8080 -p 8001:8001 -d --name jbpm-workbench quay.io/kiegroup/business-central-workbench-showcase:latest
3. https://hub.docker.com/r/jboss/kie-server-showcase
4. docker run -p 8180:8080 -d --name kie-server --link jbpm-workbench:kie-wb quay.io/kiegroup/kie-server-showcase:latest
