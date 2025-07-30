from urllib import response
from openai import base_url
import json
import base64
import time

from model.RuleModel import RuleModel
from service.AASRulesServiceImpl import AASRulesServiceImpl as RulesService
from service.AASPropertyServiceImpl import AASPropertyService as PropertyService




def execute_rule(rule, property_value=None):
    if isinstance(rule, RuleModel):
        for key, value in rule.mapping.items():
            if property_value == key:
                print(f"Executing rule for value = {property_value}, setting {rule.output_property} to {value}")
                PropertyService.update_property(rule.output_property, value)

def update_property_value(property_name, property_value):
    pre_rules = RulesService.get_pre_rules_for_property()
    for rule in pre_rules:
        execute_rule(rule, property_value)
    post_rules = RulesService.get_post_rules_for_property()
    for rule in post_rules:
        execute_rule(rule, property_value)

def main():
    subscribed_property = "QualityCheckResult"
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