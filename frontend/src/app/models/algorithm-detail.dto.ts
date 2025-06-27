import { CommentDto } from "./comment.dto";
import { UserSummaryDto } from "./user-summary.dto";
import { LanguageDetailDto } from "./language-detail.dto";

export interface AlgorithmDetailDto {
  algorithmId: number;
  title: string;
  description: string;
    codeletPath: string;
  createdAt: string;
  owner: UserSummaryDto;
  ratingCount: number;
  tags: string[];
  languages: LanguageDetailDto[];
  comments: CommentDto[];
}