export interface AlgorithmSummaryDto {
  algorithmId: number;
  title: string;
  description: string;
  createdAt: string;
  ratingCount: number;
  commentCount: number;
  tags: string[];
  languages: {
    langName: string;
    iconPath: string;
  }[];
  owner: {
    id: BigInteger;
    username: string;
    profilePic: string;
  };
}