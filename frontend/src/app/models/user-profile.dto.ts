export interface UserProfileDto {
  id: number;
  username: string;
  biography: string | null;
  createdAt: string;
  profilePic: string | null;
}