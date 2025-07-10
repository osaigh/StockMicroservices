output "resource_group" {
  value = azurerm_resource_group.aks_rg.name
}

output "aks_name" {
  value = azurerm_kubernetes_cluster.aks_cluster.name
}