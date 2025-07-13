provider "azurerm" {
  features {}
  subscription_id = var.subscription_id
}

provider "kubernetes" {
  # Assuming you're using the default kubeconfig path
  config_path = "~/.kube/config"
}

resource "azurerm_kubernetes_cluster" "aks_cluster" {
  name                = var.aks_cluster_name
  location            = var.resource_group_location
  resource_group_name = var.resource_group_name
  dns_prefix          = var.aks_cluster_dns_prefix

  default_node_pool {
    name       = "system"
    node_count = 1
    vm_size    = var.vm_size  # Uses the VM size variable
  }

  identity {
    type = "SystemAssigned"
  }

  network_profile {
    network_plugin = "azure"
  }
}

resource "azurerm_kubernetes_cluster_node_pool" "user_pool" {
  name                = "userpool"
  kubernetes_cluster_id = azurerm_kubernetes_cluster.aks_cluster.id
  vm_size            = var.vm_size
  node_count         = 1
  mode            = "User"

  node_labels = {
    "nodepool-type" = "user-nodes"
  }
}

output "kube_config" {
  value     = azurerm_kubernetes_cluster.aks_cluster.kube_config_raw
  sensitive = true
}