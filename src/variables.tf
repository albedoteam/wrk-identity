variable "src_name" {
  description = "Source name"
  type        = string
  default     = "identity-business"
}

variable "deployment_label" {
  description = "Deployment Label / Container Name"
  type        = string
  default     = "IdentityBusiness"
}

variable "secret_name" {
  description = "Secret name"
  type        = string
  default     = "identity-secrets"
}

variable "broker_connection_string" {
  description = "Broker Connection String"
  type        = string
  sensitive   = true
  default     = ""
}

variable "db_connection_string" {
  description = "Db Connection String"
  type        = string
  sensitive   = true
  default     = ""
}

variable "db_name" {
  description = "Db Name"
  type        = string
  default     = ""
}

variable "identity_server_org_url" {
  description = "Identity Server Organization URL"
  type        = string
  default     = ""
}

variable "identity_server_client_id" {
  description = "Identity Server Client Id"
  type        = string
  default     = ""
}

variable "replicas_count" {
  description = "Number of container replicas to provision."
  type        = number
  default     = 1
}