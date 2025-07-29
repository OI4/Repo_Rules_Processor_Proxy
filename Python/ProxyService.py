from urllib import response
from openai import base_url
import requests
import json
import base64
import time
from secrets.url_details import base_url, submodel_id, header


subscribed_property = "QualityCheckResult"
fake_affected_property = "StorageLocation"


def execute_rule(rule, property_value=None):
    if rule == "PostRule1":
        if property_value:
            PropertyService.update_property(fake_affected_property, "Lager")
        else:
            PropertyService.update_property(fake_affected_property, "Sperrbestand")

def update_property_value(property_name, property_value):
    pre_rules = DRulesService.get_pre_rules_for_property()
    for rule in pre_rules:
        execute_rule(rule, property_value)
    post_rules = DRulesService.get_post_rules_for_property()
    for rule in post_rules:
        execute_rule(rule, property_value)


class PropertyService:
    
    def get_property_value(property_name):
        response = requests.get(f"{base_url}/submodels/{submodel_id}/submodel-elements/{property_name}/$value", headers=header)
        if response.status_code == 200:
            return json.loads(response.content)
        else:
            raise Exception(f"Failed to get property value: {response.status_code} - {response.text}")

    def update_property(property_name, property_value):
        url_string = f"{base_url}/submodels/{submodel_id}/submodel-elements/{property_name}/$value"
        print(f"PATCH request to {url_string} with value: {json.dumps(property_value)}")
        response = requests.patch(url_string, data=json.dumps(property_value), headers=header)
        if response.status_code != 204:
            raise Exception(f"Failed to set property value: {response.status_code} - {response.text}")


class DRulesService:
    def get_pre_rules_for_property():
        # Placeholder for fetching pre-rules for a property
        return ["PreRule1", "PreRule2"]

    def get_post_rules_for_property():
        # Placeholder for fetching post-rules for a property
        return ["PostRule1", "PostRule2"]
    
class RuleModel:
    pass    # todo: what's the drules datamodel?



def main():
    last_val = None
    while True:
        current_val = PropertyService.get_property_value(subscribed_property)
        current_val = str(current_val).lower() == "true" 
        print(f"Current value of {subscribed_property}: {current_val}")

        if current_val != last_val:
            update_property_value(subscribed_property, current_val)
            last_val = current_val
        time.sleep(5)

if __name__ == "__main__":
    main()