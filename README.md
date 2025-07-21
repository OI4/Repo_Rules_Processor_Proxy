# Repo_Rules_Processor_Proxy

This software component called "Repo_Rules_Processor_Proxy" is designed in the [Open Industry 4.0 Alliance](https://openindustry4.com/) project "Exemplary industry testing of partial aspects of the proactive Asset Administration Shell". If you are interested in contributing or more details, please reach out to Christian Heinrich. (Christian.Heinrich at openindustry4.com) 

In this project we are creating a concept how to describe the behaviour of assets in their Asset Administration Shell. Therefore we are creating rules which are stored/linked in the AAS of the Asset, when the rule is evaluated to true, an action is triggered, like sending publishing an event or writing a value to a defined SubmodelElement or call an API.

So we are able to bring the complex logic, which is nowadays (hard-)coded for example in MESs (Manufacturing Execution System) to a transparent and decentralized representation.

This proxy uses in the first step the approach with describing the rules with DMN (Decision Model and Notation, see [wikipedia](https://en.wikipedia.org/wiki/Decision_Model_and_Notation)).
Those DMN-files with the rules are stored in a File-SubmodelElement in a "Behaviour-Submodel" in the AAS of the Asset. This submodel is not standardized, yet.

In the following sequence diagram the extraction of the DMN-files from the Submodel is encapsulated in the "RulesRepository".

The shown concept is not bound to DMN, you could also use another language to describe the rules. You have to ensure, the rule, the RulesParser, which extracts the paths to referenced SubmodelElement, and the RulesEngine, which evaluates the rule, are fitting together.

### The concept in a nutshell:
- There is a **Proxy** for the AAS-/submodel repository. The API_Client is not aware of the proxy, because the proxy has exact the same API-Endpoints like the AAS-/submodel-repository.
- There are **Rules**. 
    - A rule has at least one **SubmodelElementReferenceHook**, which is a reference to SubmodelElement in an AAS. Whenever this referenced SubmodelElement is touched (via a POST-, PUT-, DELETE- or maybe GET-request), the rule is relevant and must be evaluated.
    - A rule can be a **PreRequestRule** or a **PostRequestRule**.
        - A PreRequestRule is evaluated before the proxy forwards the request from the API_Client to the repository. If one PreRequestRule is evaluated to false, the request is not forwared to the repository.
        - A PostRequestRule is evaluated after the proxy has forwarded the request to the repository and if the repository has HTTP response status code 2xx.
- There is a **RulesRepository**, which provides the rules for the requested SubmodelElement. The affected rules are selected by the SubmodelElementReferenceHook of the rule and the HTTP method. There can be many rules for one SubmodelElement. This RulesRepository is filled by the "Behaviour Submodels", which hold all rules.
- There is a **DMN_RulesParser**, which extracts the paths to SubmodelElements with the current value of the SubmodelElement.
- There is a **DMN_RulesEngine**, which evaluates the rule. This might be for example [Drools](https://drools.org/).



### Sequence Diagram

![Rules Engine Sequence Diagram](/diagrams/RulesEngine_Sequence.png)