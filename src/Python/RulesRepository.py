from model.RuleModel import RuleModel

class RulesRepository:
    quality_check_rule = RuleModel(
        output_property="StorageLocation",
        mapping={True: "Lager", False: "Sperrbestand"}
    )

    pre_rules_db_inmemory = {
        "QualityCheckResult": []
    }

    post_rules_db_inmemory = {
        "QualityCheckResult": [quality_check_rule]
        }
    
    def get_pre_rules_for_property():
        return RulesRepository.pre_rules_db_inmemory.get("QualityCheckResult", [])

    def get_post_rules_for_property():
        return RulesRepository.post_rules_db_inmemory.get("QualityCheckResult", [])