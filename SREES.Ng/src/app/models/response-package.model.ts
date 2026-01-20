export interface ResponsePackage<T> {
  message: string | null;
  data: T | null;
}
