import { RedirectFunction } from "@angular/router";

export const autoRedirection : RedirectFunction = () => {
  return `/${localStorage.getItem('role') ?? "login"}`;
}
