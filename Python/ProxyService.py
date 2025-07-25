import requests
import json
import base64

base_url = "http://localhost:8081/submodels/"
submodel_id = "RuntimeData"


def execute_rule(rule):
    # todo: how to execute a rule?
    print(f"Executing rule: {rule}")

def update_property_value(property_name, property_value):
    pre_rules = DRulesService.get_pre_rules_for_property()
    for rule in pre_rules:
        execute_rule(rule)
    PropertyService.execute_post_rules(post_rules)
    post_rules = DRulesService.get_post_rules_for_property()
    for rule in post_rules:
        execute_rule(rule)


class PropertyService:
    def update_property(property_name, property_value):
        sample_string_bytes = submodel_id.encode("ascii")
        submodel_id_base64 = base64.urlsafe_b64encode(submodel_id.encode()).decode().rstrip("=")
        response = requests.get(f"{base_url}{submodel_id_base64}/submodel-elements/{property_name}/$value")
        ec_value = json.loads(response.content)


class DRulesService:
    def get_pre_rules_for_property():
        # Placeholder for fetching pre-rules for a property
        return ["PreRule1", "PreRule2"]

    def get_post_rules_for_property():
        # Placeholder for fetching post-rules for a property
        return ["PostRule1", "PostRule2"]
    
class RuleModel:
    pass    # todo: what's the drules datamodel?